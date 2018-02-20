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
} from 'semantic-ui-react'
import { RouteComponentProps } from 'react-router';


export class Quiz extends React.Component<RouteComponentProps<{}>, {}> {
    myMap = {
        0: 'Do you like new challenges & opportunities?',
        1: 'Do you speak any European language?',
        2: 'Are you interested in finance or IT?',
        3: 'Which sector is more interesting for you?',
        4: 'Do you have experience working with IT?',
        5: 'Would you like to learn a new language?',
        6: 'It\'s not a problem! Check our career page for the future possibilities.',
        7: 'Congrats! We have an offer only for you!',
        8: 'Great! Check out our careen page for IT sector jobs.',
        9: 'Yes',
        10: 'No',
        11: 'IT',
        12: 'Finance'
    }
    public render() {
        return (
            <div className='cg-prize-page'>
                <div className='cg-page-header'>
                    <Container fluid>
                        <Header as='h1' textAlign='center' inverted>
                            <Icon name='checkmark box' />
                            <Header.Content>
                                Quiz
                            </Header.Content>
                        </Header>
                    </Container>
                </div>
                <Segment style={{ padding: '1em 0em 3em' }} vertical>
                    <Container>
                        <div className='cg-title'>
                            <h2>{this.myMap[0]}</h2>
                        </div>
                        <div className="cg-quiz-button">
                            <div className='cg-action-item'>
                                <button className='cg-card-button cyan'>{this.myMap[9]}</button>
                            </div>
                            <div className='cg-action-item'>
                                <button className='cg-card-button cyan'>{this.myMap[10]}</button>
                            </div>
                        </div>
                    </Container>
                </Segment>
            </div>    
        );
    }
}