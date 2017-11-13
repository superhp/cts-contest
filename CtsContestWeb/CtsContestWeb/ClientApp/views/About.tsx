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
                            />
                            <Header
                                as='h2'
                                content='Solve coding tasks and earn points to win!'
                                inverted
                                style={{ fontSize: '1.7em', fontWeight: 'normal' }}
                            />
                        </Container>
                    </Segment>
                </div>
                <Segment style={{ padding: '1em 0em 3em' }} vertical>
                    <Container>
                        <div className='cg-title'>
                            <h2>How to participate</h2>
                        </div>
                        
                        <div className='cg-about-p'>
                            <List as='ol'>
                                <List.Item as='li'>Login using your Facebook or Google account</List.Item>
                                <List.Item as='li'>
                                    Solve coding tasks to earn points

                                    <List.Item as="ol">
                                        <List.Item as="li" value='-'>Tasks are divided into several groups based on its complexity</List.Item>
                                        <List.Item as="li" value='-'>Each task can be solved only once</List.Item>
                                        <List.Item as="li" value='-'>You can earn 15-645 points for each correct solution, depending on task’s difficulty</List.Item>
                                    </List.Item>
                                </List.Item>
                                <List.Item as='li'>Have points added to your virtual wallet</List.Item>
                                <List.Item as='li'>Buy goodies in our shopping kiosk or save it for later and win different prizes</List.Item>
                            </List>
                        </div>
                    </Container>
                    <Divider />
                    <Container>
                        <div className='cg-title'>
                            <h2>3 ways to spend your points</h2>
                        </div>
                        <div className='cg-about-p'>
                            <List as='ol'>
                                <List.Item as='li'>
                                    Purchase items in our virtual shopping booth

                                    <List.Item as="ol">
                                        <List.Item as="li" value='-'>Points can be spent in a shopping booth only on the same day it is gained</List.Item>
                                        <List.Item as="li" value='-'>Unspent points can be added up to your total score in a daily and in a conference leaderboard</List.Item>
                                        <List.Item as="li" value='-'>Each registered contestant can purchase the same item only once</List.Item>
                                        <List.Item as="li" value='-'>Points spent in either virtual shopping booth or to win daily prize are no longer valid</List.Item>
                                    </List.Item>
                                </List.Item>
                                <List.Item as='li'>
                                    Save your points to win cool daily prize

                                    <List.Item as="ol">
                                        <List.Item as="li" value='-'>Daily prize is awarded to contestant based on leaderboard results</List.Item>
                                        <List.Item as="li" value='-'>
                                            Final value of a daily prize is all unspent points earned on a particular day

                                            <List.Item as='ol'>
                                                <List.Item as="li" value='o'>i.e. getting daily prize sets daily balance to 0</List.Item>
                                            </List.Item>
                                        </List.Item>

                                        <List.Item as="li" value='-'>Daily leaderboard calculates points that are gained on a particular day only</List.Item>
                                        <List.Item as="li" value='-'>The contestant who is eligible to receive daily prize may choose to pass it on to the next best contestant and keep the points for a conference leaderboard</List.Item>
                                        <List.Item as="li" value='-'>Each registered contestant can win only one daily prize during the conference</List.Item>
                                        <List.Item as="li" value='-'>Points spent to win daily prize are no longer valid </List.Item>
                                    </List.Item>
                                </List.Item>
                            </List>
                        </div>

                        <p className='cg-about-p'>Daily prizes</p>
                        <div className='cg-about-p'>

                            <List as='ol'>
                                <List.Item as='li'>
                                    Save your points to win Sony PlayStation 4

                                    <List.Item as="ol">
                                        <List.Item as="li" value='-'>Conference prize is awarded to contestant based on leaderboard results</List.Item>
                                        <List.Item as="li" value='-'>Conference leaderboard calculates points that are gained and not spent throughout the 3 conference days </List.Item>
                                        <List.Item as="li" value='-'>PlayStation 4 with 2 Controllers + FIFA 18 will be awarded to the contestant with the highest total balance – the sum of participant’s unspent points throughout the entire conference</List.Item>
                                        <List.Item as="li" value='-'>
                                            Contestant must be present at the conference during the award ceremony just before the closing keynote @Alfa at 5:10 PM on Friday

                                            <List.Item as='ol'>
                                                <List.Item as="li" value='o'>In case of absence, the prize will go to the next best in the total conference leaderboard</List.Item>
                                            </List.Item>
                                        </List.Item>

                                        <List.Item as="li" value='-'>Daily leaderboard calculates points that are gained on a particular day only</List.Item>
                                        <List.Item as="li" value='-'>The contestant who is eligible to receive daily prize may choose to pass it on to the next best contestant and keep the points for a conference leaderboard</List.Item>
                                        <List.Item as="li" value='-'>Each registered contestant can win only one daily prize during the conference</List.Item>
                                        <List.Item as="li" value='-'>Points spent to win daily prize are no longer valid </List.Item>
                                    </List.Item>
                                </List.Item>
                            </List>
                        </div>
                        <p className='cg-about-p'>Visit Cognizant stand to check leaderboard and to shop</p>
                    </Container>
                    <Divider />
                    <Container>
                         <div className='cg-title'>
                            <h2>Shopping</h2>
                         </div>
                         <p className='cg-about-p'>
                             Once you earn enough points and want to purchase an item
                        </p>
                         <div className='cg-about-p'>
                            <List as='ol'>

                                <List.Item as="ol">
                                    <List.Item as="li" value='-'>Go to virtual shopping booth</List.Item>
                                    <List.Item as="li" value='-'>
                                        Select an item, click Buy

                                        <List.Item as='ol'>
                                            <List.Item as="li" value='o'>Click on details to read more about it</List.Item>
                                        </List.Item>
                                    </List.Item>

                                    <List.Item as="li" value='-'> Receive a unique QR code (this is your purchase receipt)</List.Item>
                                    <List.Item as="li" value='-'>Present the code at Cognizant stand to redeem the prize</List.Item>
                                </List.Item>

                            </List>
                        </div>
                    </Container>

                    <Divider />
                    <Container>
                         <div className='cg-title'>
                            <h2>Other rules</h2>
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
