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


import Questions from './Questions';

export class Quiz extends React.Component<RouteComponentProps<{}>, {}> {

    public render() {
        return (
            <div className='cg-prize-page'>
                <div className='cg-page-header'>
                    <div className='cg-page-header-overlay'>
                        <Container fluid>
                            <Header as='h1' textAlign='center' inverted>
                                <Icon name='checkmark box' />
                                <Header.Content>
                                    Quiz
                                </Header.Content>
                            </Header>
                        </Container>
                    </div>
                </div>
                <Questions />
            </div>    
        );
    }
}