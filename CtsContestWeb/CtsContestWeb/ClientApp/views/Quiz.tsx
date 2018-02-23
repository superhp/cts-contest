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

import q from './Graph'; 

export class Quiz extends React.Component<RouteComponentProps<{}>, {}> {
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
                            <h2>{q.curr.text}</h2>
                        </div>
                        <div className="cg-quiz-button">
                            {q.curr.edges.map(x =>
                                <div className='cg-action-item'>
                                    <button className='cg-card-button cyan'>{x.text}</button>
                                </div>
                            )}
                        </div>
                    </Container>
                </Segment>
            </div>    
        );
    }
}