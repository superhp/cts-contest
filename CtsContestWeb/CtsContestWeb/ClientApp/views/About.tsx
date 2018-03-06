import * as React from 'react';
import { RouteComponentProps } from 'react-router';

// import * as GA from 'react-ga';
// GA.initialize('UA-109707377-1');

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

    componentWillMount() {        
        //GA.pageview(window.location.pathname + window.location.search);
    }

    render() {
        const visible = this.state.visible;

        return (
            <div>
                <div className='cg-about-header'>
                    <Segment vertical textAlign='center'>
                        <Container text >
                            <Header className='cg-about-header-text'
                                as='h1'
                                content='Cognizant TaskCracker'
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
                            <ol>
                                <li>Login using your Facebook or Google account</li>
                                <li>Solve coding tasks to earn points
                                    <ul className='cg-ul-dash' style={{paddingLeft: 25}}>
                                        <li>Tasks are divided into several groups based on its complexity</li>
                                        <li>Each task can be solved only once</li>
                                        <li>You can earn 15-645 points for each correct solution, depending on task’s difficulty</li>
                                    </ul>
                                </li>
                                <li>Have points added to your virtual wallet</li>
                                <li>Buy goodies in our shopping booth or save it for later and win different prizes</li>
                            </ol>
                        </div>
                    </Container>
                    <Divider />
                    <Container>
                        <div className='cg-title'>
                            <h2>2 ways to spend your points</h2>
                        </div>
                        <div className='cg-about-p cg-points'>

                            <div className='cg-title-bold'>1.  Purchase items in our virtual shopping booth</div>
                            <ul className='cg-ul-dash'>
                                <li>Points can be spent in a shopping booth</li>
                                <li>Unspent points can be added up to your total score in a daily leaderboard</li>
                                <li>Each registered contestant can purchase the same item only once </li>
                                <li>Points spent in either virtual shopping booth or to win daily prize are no longer valid</li>
                            </ul>

                            <div className='cg-title-bold'>2.  Save your points to win cool daily prize</div>
                            <ul className='cg-ul-dash'>
                                <li>Daily prize is awarded to contestant based on leaderboard results</li>
                                <li>Points spent to win daily prize are no longer valid</li>
                                <li><strong>Boompods Enduro Sportpods Bluetooth Earphones</strong> will be awarded to the contestant with the highest total balance – the sum of participant’s unspent points throughout the entire game</li>
                                <li>In case of absence, the prize will go to the next best in the leaderboard</li>
                            </ul>

                            <div className='cg-image main-prize' >
                                <img src="../Main_prize.png" alt="The main prize" />
                            </div>
                            <p>Visit Cognizant stand to check leaderboard and to shop</p>
                        </div>
                    </Container>

                    <Divider />
                    <Container>
                        <div className='cg-title'>
                            <h2>Shopping</h2>
                        </div>
                        <p className='cg-about-p' style={{paddingBottom: 5}}>
                            Once you earn enough points and want to purchase an item:
                        </p>
                        <div className='cg-about-p'>
                            <ul className='cg-ul-dash' style={{ paddingLeft: 25 }}>
                                <li>Go to virtual shopping booth</li>
                                <li>Select an item, click Buy
                                    <ul className='cg-ul-circle'>
                                        <li>Click on details to read more about it</li>
                                    </ul>
                                </li>
                                <li>Receive a unique QR code (this is your purchase receipt)</li>
                                <li>Present the code at Cognizant stand to redeem the prize</li>
                            </ul>
                        </div>
                    </Container>

                    <Divider />
                    <Container>
                        <div className='cg-title'>
                            <h2>Other rules</h2>
                        </div>
                        <div className='cg-about-p'>
                            <ul className='cg-ul-dash' style={{ paddingLeft: 25 }}>
                                <li>You can code using any of the following programming languages:
                                    <div className='row'>
                                        <div className='col-xs-3'>
                                            <ul>
                                                <li>C</li>
                                                <li>Clojure</li>
                                                <li>COBOL</li>
                                                <li>C++</li>
                                                <li>C#</li>
                                                <li>D</li>
                                        </ul>
                                        </div>
                                        <div className='col-xs-3'>
                                             <ul>
                                                <li>Erland</li>
                                                <li>GO</li>
                                                <li>Haskell</li>
                                                <li>Java</li>
                                                <li>Perl</li>
                                                <li>PHP</li>
                                            </ul>
                                        </div>
                                        <div className='col-xs-3'>
                                             <ul>
                                                <li>Python 2</li>
                                                <li>Python 3</li>
                                                <li>R</li>
                                                <li>Ruby</li>
                                                <li>Scala</li>
                                                <li>Swift</li>
                                            </ul>
                                        </div>
                                    </div>
                                </li>
                                <li>The execution time of each solution must not exceed 5 seconds and it must not consume more than 256 MB of memory. The size of the source file of a solution must not exceed 256 KB.</li>
                                <li>There is an editor provided near each of the tasks, but you can use anything you like – just make sure to put your code into the editor before you submit.</li>
                                <li>You can try to submit your solution as many times as you want. Once your code passes the tests, you will gain the respective amount of points and the task will be marked as solved.</li>
                                <li>Quantity of items in stock is limited.</li>
                                <li>Cognizant employees are not eligible to participate.</li>
                            </ul>
                        </div>
                    </Container>
                </Segment>
            </div>
        )
    }
}
