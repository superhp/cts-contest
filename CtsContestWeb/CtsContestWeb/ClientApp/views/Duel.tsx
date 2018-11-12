import * as React from 'react';
import {Container, Divider, Grid, Header, Icon} from 'semantic-ui-react';
import * as signalR from '@aspnet/signalr';
import {DuelTask} from './DuelTask';
import {fakeDuelInfo} from '../mocks/fakeData';
import {CompileResult} from '../components/models/Task';
import {UserInfo} from '../components/models/UserInfo';
import {DuelInfo, DuelTime} from '../components/models/DuelInfo';
import InitDuelButton from '../components/InitDuelButton';
import DuelRules from '../components/DuelRules';

interface DuelState {
    compileResult: CompileResult | null,
    winner: UserInfo | null,
    step: string,
    duelInfo: DuelInfo,
    secondsToPlay: number,
    compiling: boolean,
    time: DuelTime,
    totalWins: number,
    totalLooses: number,
    waitingPlayers: number,
    activePlayers: number,
    isInDuel: boolean
}

export class Duel extends React.Component<any, DuelState> {

    hubConnection: signalR.HubConnection;
    timer: any;
    statisticsTimer: any;


    constructor(props: any) {
        super(props);

        this.state = {
            compileResult: null,
            winner: null,
            step: 'initial',
            duelInfo: fakeDuelInfo,
            secondsToPlay: 0,
            compiling: false,
            time: {minutes: 0, seconds: 0},
            waitingPlayers: 0,
            totalWins: 0,
            totalLooses: 0,
            activePlayers: 0,
            isInDuel: false
        };

        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl('/Duelhub')
            .configureLogging(signalR.LogLevel.Information)
            .build();
        console.log("Component mounted. Step: initial");

        this.hubConnection.on("waitingPlayers", (players: number) => {
            this.setState({waitingPlayers: players});
            console.log("Number of waiting players received");
        });

        this.hubConnection.on("DuelStarts", (duelInfo: DuelInfo) => {
            this.setState({step: 'started', duelInfo: duelInfo, compiling: false, compileResult: null});
            this.resetDuelTimerState();
            this.timer = setInterval(() => {
                this.countDown();
            }, 1000);
            console.log("Game started. Step: started");
        });

        this.hubConnection.on("solutionChecked", (compileResult: CompileResult) => {
            this.setState({compileResult: compileResult, compiling: false});
            console.log('Submitted code did not go through.');
        })

        this.hubConnection.on("DuelHasWinner", (winningPlayer: UserInfo) => {
            this.setState({
                step: 'finishedByWinning',
                winner: winningPlayer,
                compiling: false,
                compileResult: null,
                isInDuel: false
            });
            console.log(`${winningPlayer.email} won. Cause: correct solution. Step: 'finishedByWinning'`)
        })

        this.hubConnection.on("scoreAdded", (score: number) => {
            this.props.onIncrementBalance(this.state.duelInfo.task.value);
            console.log(score + ' coins added');
        })

        this.hubConnection.on("closeThisWindow", () => {
            this.setState({step: 'closeWindow'});
            console.log("Duel is already ongoing. Step: 'closeWindow'");
        })

        this.hubConnection.on("opponentDisconnected", (winningPlayer: UserInfo) => {
            this.props.onIncrementBalance(this.state.duelInfo.task.value);
            this.setState({step: 'finishedByDisconnection', winner: winningPlayer})
            console.log(`${winningPlayer.email} won. Cause: opponent disconnection. Step: 'finishedByDisconnection'`)
        })
    }

    countDown() {
        let now = new Date();
        let start = new Date(this.state.duelInfo.startTime.toString());
        let timeElapsed = now.getTime() - start.getTime();
        let seconds = Math.max(this.state.duelInfo.duration * 60 - timeElapsed / 1000, 0);
        let time = this.secondsToTime(seconds);
        this.setState({time: time, secondsToPlay: seconds});

        if (seconds == 0 && !this.state.winner) {
            this.setState({step: "finishedByTimeout", isInDuel: false});
            this.resetDuelTimerState();
            this.componentWillUnmount();
        }
    }

    resetDuelTimerState() {
        clearInterval(this.timer);
        this.setState({secondsToPlay: 0, time: this.secondsToTime(0)});
    }

    secondsToTime(secs: number) {
        let divisor_for_minutes = secs % (60 * 60);
        let minutesVal = Math.floor(divisor_for_minutes / 60);

        let divisor_for_seconds = divisor_for_minutes % 60;
        let secondsVal = Math.ceil(divisor_for_seconds);

        let obj: DuelTime = {
            minutes: minutesVal,
            seconds: secondsVal
        };
        return obj;
    }

    componentWillUnmount() {
        this.hubConnection.stop()
            .then(() => console.log('Connection terminated'));
        clearInterval(this.timer);
        clearInterval(this.statisticsTimer);
    }

    componentDidMount() {
        if (this.props.userInfo.isLoggedIn) this.hasUserAnyDuelTasksLeft();
        this.statisticsTimer = setInterval(this.updateDuelStatistics, 10 * 1000); // 10 seconds

        this.updateDuelStatistics();
    }

    hasUserAnyDuelTasksLeft = () => {
        fetch('api/user/has-duel-tasks-left', {credentials: 'include'}
        ).then(response => response.json() as Promise<any>)
            .then((data: boolean) => {
                if (data === false) {
                    this.setState({step: 'noTasksLeft'})
                }
            });
    }

    updateDuelStatistics = () => {
        fetch('api/user/duel-statistics', {
            credentials: 'include'
        }).then(response => response.json() as Promise<any>)
            .then(data => {
                this.setState({
                    totalWins: data.totalWins,
                    totalLooses: data.totalLooses,
                    activePlayers: data.activePlayers,
                    isInDuel: data.isInDuel
                });
            });
    }

    findOpponent = () => {
        this.hubConnection
            .start()
            .then(() => console.log('Established connection.'))
            .catch((err: any) => console.log('Error while establishing connection :('));
        console.log("Trying to start connection. Step: searching");
        this.setState({step: 'searching'});
    }

    submitSolution = (code: string, language: number) => {
        this.setState({compiling: true});
        this.hubConnection.send("CheckSolution", code, language)
            .then(() => console.log(`Called method 'CheckSolution'. Waiting for response.`));
    }

    getCurrentStepTemplate = (step: string) => {
        switch (step) {
            case 'noTasksLeft':
                return <div className="cg-title loading-text">
                    <h3>You have already played all Clash-of-Code tasks </h3>
                </div>;
            case 'initial':
                return <InitDuelButton name={this.state.isInDuel ? "Resume" : "Start"}
                                       findOpponent={this.findOpponent}/>;
            case 'searching':
                return <div className="cg-title loading-text">
                    <h3>Wait for your opponent...</h3>
                </div>;
            case 'started':
                return <DuelTask info={this.state.duelInfo} submitSolution={this.submitSolution}
                                 compilerError={this.state.compileResult}
                                 compiling={this.state.compiling}/>
            case 'finishedByWinning':
                return <div>
                    <div className="cg-title loading-text"><h3>{this.state.winner && this.state.winner.name} has won the Duel!</h3></div>
                    <div className="task-points">Task's value was: {this.state.duelInfo.task.value} coins</div>
                    <InitDuelButton name="Play again" findOpponent={this.findOpponent}/>
                </div>
            case 'finishedByDisconnection':
                return <div>
                    <div className="cg-title loading-text"><h3>Opponent disconnected - you won!</h3></div>
                    <InitDuelButton name="Play again" findOpponent={this.findOpponent}/>
                </div>;
            case 'finishedByTimeout':
                return <div>
                    <div className="cg-title loading-text"><h3>Time for a duel has finished. It's a draw! </h3></div>
                    <InitDuelButton name="Play again" findOpponent={this.findOpponent}/>;
                </div>
            case 'closeWindow':
                return <div className="cg-title loading-text">
                    <h3>Duel is ongoing. Close this window</h3>
                </div>;
        }
    }

    public render() {
        return (
            <div className='cg-prize-page'>
                <div className='cg-page-header'>
                    <div className='cg-page-header-overlay'>
                        <Container fluid>
                            <Header as='h1' textAlign='center' inverted>
                                <Icon name='checkmark box'/>
                                <Header.Content>
                                    Clash of Code
                                </Header.Content>
                            </Header>
                        </Container>
                    </div>
                </div>

                {
                    this.state.step === "started" ?
                        <RulesAndDuelInfo info={this.state.duelInfo} duelState={this.state}
                                          taskName={this.state.duelInfo.task.name}
                                          taskPoints={this.state.duelInfo.task.value}/> :
                        <DuelRules duelState={this.state} />
                }


                {
                    this.props.userInfo.isLoggedIn ?
                        this.getCurrentStepTemplate(this.state.step) :
                        <Container textAlign="center">
                            <div className="error-message cg-about-p">Please login before participating in duel</div>
                        </Container>
                }

            </div>
        );
    }
}

const RulesAndDuelInfo =
    ({info, duelState, taskName, taskPoints}: { info: DuelInfo, duelState: DuelState, taskName: string, taskPoints: number }) => {
        return (<Container fluid>
            <Grid columns={1} relaxed>
                <Grid.Column mobile={16} tablet={16} computer={16} style={{textAlign: 'center'}}>
                    <Container>
                        <div className='cg-about-p'>
                            <p>
                                <strong>{info.players[0].name} Wins: {info.players[0].totalWins} /
                                    Loses: {info.players[0].totalLooses}</strong>
                                <img src='https://static.thenounproject.com/png/161955-200.png'
                                     style={{height: '80px'}}></img>
                                <strong>{info.players[1].name} Wins: {info.players[1].totalWins} /
                                    Loses: {info.players[1].totalLooses}</strong>
                            </p>
                            <p>Time left: {duelState.time.minutes} m. {duelState.time.seconds} sec.</p>
                        </div>
                    </Container>
                </Grid.Column>
            </Grid>
        </Container>)
    }