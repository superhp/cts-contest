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
                <div style={{ backgroundColor: '#BBDEFB' }}>
                    <Segment vertical textAlign='center'>
                        <Container text >
                            <Header
                                as='h1'
                                content='Programming contest' 
                                //inverted
                                style={{ fontSize: '4em', fontWeight: 'arial', marginBottom: 0, marginTop: '0.5em' }}
                            />
                            <Header
                                as='h2'
                                content='Do whatever you want when you want to.'
                                //inverted
                                style={{ fontSize: '1.7em', fontWeight: 'normal' }}
                            />
                            <Button color='blue' size='huge'>
                                Get Started
                                <Icon name='right arrow' />
                            </Button>
                            <div style={{ height: 50}} />
                        </Container>
                    </Segment>
                </div>
                <Segment style={{ padding: '4em 0em' }} vertical>
                    <Container>
                        <Header as='h3' textAlign='center' style={{ fontSize: '2em' }}>About</Header>
                        <p style={{ fontSize: '1.33em' }}>
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                            Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
                            Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
                            Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                        </p>
                    </Container>
                    <Divider
                        style={{ margin: '3em 0em'}}
                    />
                    <Container>
                        <Header as='h3' textAlign='center' style={{ fontSize: '2em' }}>Prizes</Header>
                         <p style={{ fontSize: '1.33em' }}>
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                            Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
                            Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
                            Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                        </p>
                    </Container>
                    <Divider
                        style={{ margin: '3em 0em'}}
                    />
                    <Container>
                        <Header as='h3' textAlign='center' style={{ fontSize: '2em' }}>How to compete?</Header>
                        <div style={{ fontSize: '1.33em'}}>
                            <List bulleted>
                                <List.Item>Log in</List.Item>
                                <List.Item>Solve puzzles</List.Item>
                                <List.Item>Gain currency</List.Item>
                                <List.Item>Buy prizes</List.Item>
                            </List>
                        </div>
                    </Container>

                    <Divider
                        style={{ margin: '3em 0em'}}
                    />
                    <Container>
                        <Header as='h3' textAlign='center' style={{ fontSize: '2em' }}>Rules</Header>
                         <p style={{ fontSize: '1.33em' }}>
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
