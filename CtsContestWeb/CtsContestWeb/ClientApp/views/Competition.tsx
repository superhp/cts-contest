import * as React from 'react';
import {
    Button,
    Container,
    Divider,
    Grid,
    Header,
    Icon,
    Image,
    List,
    Menu,
    Segment,
    Visibility,
} from 'semantic-ui-react';

import { RouteComponentProps } from 'react-router';
import * as signalR from '@aspnet/signalr';

export class Competition extends React.Component<RouteComponentProps<{}>, {}> {

    hubConnection: signalR.HubConnection;

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

        this.hubConnection.on("competitionStarts", () => {
            console.log("competition starts");
        });
    }

    public render() {
        return (
            <div className='cg-prize-page'>
                <div className='cg-page-header'>
                    <Container fluid>
                        <Header as='h1' textAlign='center' inverted>
                            <Icon name='checkmark box' />
                            <Header.Content>
                                Competition
                            </Header.Content>
                        </Header>
                    </Container>
                </div>

                <div>How to play...</div>
                <div>Button "Find opponent"</div>

                <div className="cg-title loading-text">
                    <h2>Wait for your opponent...</h2>
                </div>
            </div>    
        );
    }
}