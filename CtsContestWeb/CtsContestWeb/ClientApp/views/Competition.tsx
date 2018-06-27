import * as React from 'react';
import {
    Container,
    Divider,
    Header,
    Icon
} from 'semantic-ui-react';

import { RouteComponentProps } from 'react-router';
import * as signalR from '@aspnet/signalr';
import { CompetitionTask } from './CompetitionTask';
import { fakeCompetitionInfo } from '../mocks/fakeData';
import { CompileResult } from '../components/models/Task';
import { UserInfo } from '../components/models/UserInfo';
import { CompetitionInfo } from '../components/models/CompetitionInfo';

interface CompetitionState {
    compileResult: CompileResult | null,
    winner: UserInfo | null,
    step: string,
    competitionInfo: CompetitionInfo
}

export class Competition extends React.Component<RouteComponentProps<{}>, CompetitionState> {

    hubConnection: signalR.HubConnection;

    constructor(props: any) {
        super(props);

        this.state = {
            compileResult: null,
            winner: null,
            step: 'initial',
            competitionInfo: fakeCompetitionInfo
        };

        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl('/competitionhub')
            .configureLogging(signalR.LogLevel.Information)
            .build();
        console.log("mounted");
    }

    findOpponent = () => {
        this.hubConnection
            .start()
            .then(() => console.log('Connection started!'))
            .catch((err:any) => console.log('Error while establishing connection :('));
        console.log("searching");
        this.setState({ step: 'searching' });

        this.hubConnection.on("competitionStarts", (competitionInfo: CompetitionInfo) => {
            this.setState({step: 'started', competitionInfo: competitionInfo});
            console.log("started game");
        });

        this.hubConnection.on("solutionChecked", (compileResult: CompileResult) => {
            this.setState({compileResult: compileResult});
            console.log('compiler error received');
        })

        this.hubConnection.on("competitionHasWinner", (winningPlayer: UserInfo) => {
            this.setState({step: 'finished', winner: winningPlayer});
            console.log(`${winningPlayer.email} won`)
        })
    }

    submitSolution = (code: string, language: number) => {
        this.hubConnection.invoke("CheckSolution", code, language)
            .then(() => console.log('invoked method CheckSolution'));
    }

    getCurrentStepTemplate = (step: string) => {
        switch (step) {
            case 'initial':
                return <Container textAlign="center">
                           <button className='cg-card-button cyan' onClick={this.findOpponent} style={{
                               "width": "15%"
                           }}>Start</button>
                </Container>;
            case 'searching':
                return <div className="cg-title loading-text">
                           <h2>Wait for your opponent...</h2>
                </div>;
            case 'started':
                return <CompetitionTask info={this.state.competitionInfo} submitSolution={this.submitSolution} compilerError={this.state.compileResult}/>
            case 'finished':
                return <div className="cg-title loading-text">
                    <h2>{this.state.winner && this.state.winner.name} has won the duel!</h2>
                </div>;
        }
    }

    public render() {
        return (
            <div className='cg-prize-page'>
                <div className='cg-page-header'>
                    <Container fluid>
                        <Header as='h1' textAlign='center' inverted>
                            <Icon name='checkmark box' />
                            <Header.Content>
                                Duel
                            </Header.Content>
                        </Header>
                    </Container>
                </div>

                <Rules/>
                <Divider />

                {this.getCurrentStepTemplate(this.state.step)}

                
            </div>    
        );
    }
}

const Rules = ({}) => {
    return (
        <Container>
            <div className='cg-title'>
                <h2>Rules</h2>
            </div>

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