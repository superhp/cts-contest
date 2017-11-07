import * as React from 'react';
import { RouteComponentProps } from 'react-router';

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

export class About extends React.Component<RouteComponentProps<{}>, {}> {
    state = { visible: false }

    hideFixedMenu = () => this.setState({ visible: false })
    showFixedMenu = () => this.setState({ visible: true })

    render() {
        const visible = this.state.visible;

        return (
            <div>
                <div className='cg-about-header'>
                    <Segment vertical textAlign='center'>
                        <Container text >
                            <Header className='cg-about-header-text'
                                as='h1'
                                content='PROGRAMMING CONTEST' 
                                inverted
                                style={{ fontSize: '3em', fontWeight: 'arial', marginBottom: 0 }}
                            />
                            <Header
                                as='h2'
                                content='Compete and win prizes.'
                                inverted
                                style={{ fontSize: '1.7em', fontWeight: 'normal' }}
                            />
                        </Container>
                    </Segment>
                </div>
                <Segment style={{ padding: '1em 0em 3em' }} vertical>
                    <Container>
                        <div className='cg-title'>
                            <h2>About</h2>
                        </div>
                        
                        <p className='cg-about-p'>
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                            Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
                            Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
                            Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                        </p>
                    </Container>
                    <Divider />
                    <Container>
                        <div className='cg-title'>
                            <h2>Prizes</h2>
                        </div>
                         <p className='cg-about-p'>
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                            Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
                            Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
                            Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                        </p>
                    </Container>
                    <Divider />
                    <Container>
                         <div className='cg-title'>
                            <h2>How to compete?</h2>
                        </div>
                        <div className='cg-about-p'>
                            <List bulleted>
                                <List.Item>Log in</List.Item>
                                <List.Item>Solve puzzles</List.Item>
                                <List.Item>Gain currency</List.Item>
                                <List.Item>Buy prizes</List.Item>
                            </List>
                        </div>
                    </Container>

                    <Divider />
                    <Container>
                         <div className='cg-title'>
                            <h2>Rules</h2>
                        </div>
                         <p className='cg-about-p'>
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                            Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
                            Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
                            Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                        </p>
                    </Container>
                </Segment>
            </div>
        )
    }
}
