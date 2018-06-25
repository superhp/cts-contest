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

interface CompetitionState {
    step: string
}

export class Competition extends React.Component<RouteComponentProps<{}>, CompetitionState> {

    hubConnection: signalR.HubConnection;
    constructor(props: any) {
        super(props);

        this.state = {
            step: 'initial'
        };
    }

    public componentDidMount() {
        console.log("mounted");
    
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl('http://localhost:2531/competition')
            .configureLogging(signalR.LogLevel.Information)
            .build();
    
        this.hubConnection
            .start()
            .then(() => console.log('Connection started!'))
            .catch(err => console.log('Error while establishing connection :('));

        this.hubConnection.on("competitionStarts", (competitionInfo) => {
            console.log("competition starts");
            console.log(competitionInfo);
        });
    }

    findOpponent = () => {
        console.log('Start searching now');
        this.setState({ step: 'searching' });
        setTimeout(() => this.setState({step: 'started'}), 2000);
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
                return <CompetitionTask taskId={5}/>
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