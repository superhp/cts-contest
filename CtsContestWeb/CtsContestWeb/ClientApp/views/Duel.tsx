import * as React from 'react';
import {Container, Divider, Grid, Header, Icon} from 'semantic-ui-react';
import * as signalR from '@aspnet/signalr';
import {DuelTask} from './DuelTask';
import {fakeDuelInfo} from '../mocks/fakeData';
import {CompileResult} from '../components/models/Task';
import {UserInfo} from '../components/models/UserInfo';
import {DuelInfo, DuelTime, initialDuelTime} from '../components/models/DuelInfo';

interface DuelState {
    compileResult: CompileResult | null,
    winner: UserInfo | null,
    step: string,
    DuelInfo: DuelInfo,
    secondsToPlay: number,
    compiling: boolean,
    time: DuelTime,
    totalWins: number,
    totalLooses: number,
    waitingPlayers: number
}

export class Duel extends React.Component<any, DuelState> {

    hubConnection: signalR.HubConnection;
    timer: any;


    constructor(props: any) {
        super(props);

        this.state = {
            compileResult: null,
            winner: null,
            step: 'initial',
            DuelInfo: fakeDuelInfo,
            secondsToPlay: initialDuelTime.minutes * 60 + initialDuelTime.seconds,
            compiling: false,
            time: initialDuelTime,
            waitingPlayers: 0,
            totalWins: 0,
            totalLooses: 0
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

        this.hubConnection.on("DuelStarts", (DuelInfo: DuelInfo) => {
            this.setState({step: 'started', DuelInfo: DuelInfo});
            this.resetDuelTimerState();
            this.timer = setInterval(() => {
                this.countDown()
            }, 1000);
            console.log("Game started. Step: started");
        });

        this.hubConnection.on("solutionChecked", (compileResult: CompileResult) => {
            this.setState({compileResult: compileResult, compiling: false});
            console.log('Submitted code did not go through.');
        })

        this.hubConnection.on("DuelHasWinner", (winningPlayer: UserInfo) => {
            this.setState({step: 'finishedByWinning', winner: winningPlayer});
            console.log(`${winningPlayer.email} won. Cause: correct solution. Step: 'finishedByWinning'`)
        })

        this.hubConnection.on("scoreAdded", (score: number) => {
            this.props.onIncrementBalance(this.state.DuelInfo.task.value);
            console.log(score + ' points added');
        })

        this.hubConnection.on("closeThisWindow", () => {
            this.setState({step: 'closeWindow'});
            console.log("Duel is already ongoing. Step: 'closeWindow'");
        })

        this.hubConnection.on("opponentDisconnected", (winningPlayer: UserInfo) => {
            this.props.onIncrementBalance(this.state.DuelInfo.task.value);
            this.setState({step: 'finishedByDisconnection', winner: winningPlayer})
            console.log(`${winningPlayer.email} won. Cause: opponent disconnection. Step: 'finishedByDisconnection'`)
        })
    }

    countDown() {
        let seconds = this.state.secondsToPlay - 1;
        this.setState({
            time: this.secondsToTime(seconds),
            secondsToPlay: seconds,
        });

        if (seconds == 0) {
            this.setState({step: "finishedByTimeout"});
            this.resetDuelTimerState();
            this.componentWillUnmount();
        }
    }

    resetDuelTimerState() {
        clearInterval(this.timer);
        this.setState({secondsToPlay: initialDuelTime.minutes * 60 + initialDuelTime.seconds, time: initialDuelTime});
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
    }

    componentDidMount() {
        fetch('api/user/duel-statistics', {
            credentials: 'include'
        }).then(response => response.json() as Promise<any>)
            .then(data => {
                this.setState({totalWins: data.totalWins, totalLooses: data.totalLooses});
            });

        let timeLeft = this.secondsToTime(this.state.secondsToPlay);
        this.setState({time: timeLeft});
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
            case 'initial':
                return <Container textAlign="center">
                    <div>
                        <button className='cg-card-button cyan' onClick={this.findOpponent} style={{
                            "width": "15%"
                        }}>Start
                        </button>
                    </div>
                    ;</Container>;
            case 'searching':
                return <div className="cg-title loading-text">
                    <h2>Wait for your opponent...</h2>
                    {this.state.waitingPlayers != 1 ? <h3>{this.state.waitingPlayers} opponents are in queue</h3> : <h3>{this.state.waitingPlayers} opponent is in queue</h3> }
                </div>;
            case 'started':
                return <DuelTask info={this.state.DuelInfo} submitSolution={this.submitSolution}
                                 compilerError={this.state.compileResult}
                                 compiling={this.state.compiling}/>
            case 'finishedByWinning':
                return <div className="cg-title loading-text">
                    <h2>{this.state.winner && this.state.winner.name} has won the Duel!</h2>
                </div>;
            case 'finishedByDisconnection':
                return <div className="cg-title loading-text">
                    <h2>Opponent disconnected - you won!</h2>
                </div>;
            case 'finishedByTimeout':
                return <div>
                    <div className="cg-title loading-text"><h2>Time for a duel has finished. There is no winner! It's a
                        draw! </h2></div>
                    ;
                    <Container textAlign="center">
                        <div>
                            <button className='cg-card-button cyan' onClick={this.findOpponent} style={{
                                "width": "15%"
                            }}>Start
                            </button>
                        </div>
                        ;</Container>;
                </div>
            case 'closeWindow':
                return <div className="cg-title loading-text">
                    <h2>Duel is ongoing. Close this window</h2>
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
                        <RulesAndDuelInfo info={this.state.DuelInfo} duelState={this.state}
                                          taskName={this.state.DuelInfo.task.name}
                                          taskPoints={this.state.DuelInfo.task.value}/> :
                        <Rules centered={true} duelState={this.state}/>
                }
                <Divider/>

                {this.getCurrentStepTemplate(this.state.step)}


            </div>
        );
    }
}

const Rules = ({centered, duelState}: { centered: boolean, duelState: DuelState }) => {

    return (
        <Container>
            <div className='cg-about-p'>
                <div>Total wins: {duelState.totalWins}. Total looses: {duelState.totalLooses}</div>
            </div>
            {
                centered ?
                    <div className='cg-title'>
                        <h2>Rules</h2>
                    </div> :
                    <Header as='h1' textAlign='left'>
                        <Header.Content>
                            RULES
                        </Header.Content>
                    </Header>
            }
            <div className='cg-about-p'>
                <ol>
                    <li>Wait to get matched with an opponent</li>
                    <li>Solve a randomly chosen task</li>
                    <li>Submit the correct solution faster than your opponent and win the duel!</li>
                </ol>
            </div>
        </Container>
    );
}

const RulesAndDuelInfo = ({info, duelState, taskName, taskPoints}: { info: DuelInfo, duelState: DuelState, taskName: string, taskPoints: number }) => {
    return (<Container fluid>
        <Grid columns={2} relaxed>

            <Grid.Column mobile={16} tablet={8} computer={8}>
                <Rules centered={false} duelState={duelState}/>
            </Grid.Column>
            <Grid.Column mobile={16} tablet={8} computer={8}>
                <Container>
                    <Header as='h1' textAlign='left'>
                        <Header.Content>
                            INFORMATION
                        </Header.Content>
                    </Header>

                    <div className='cg-about-p'>
                        <p><strong>{info.players[0].name}</strong> vs <strong>{info.players[1].name}</strong></p>
                        <p>Time remaining: {duelState.time.minutes} m. {duelState.time.seconds} sec.</p>
                        <p>Task's {taskName} value: {taskPoints} points</p>
                    </div>
                </Container>
            </Grid.Column>
        </Grid>
    </Container>)
}