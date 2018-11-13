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
                    <div className='cg-about-header-overlay'>
                        <Segment vertical textAlign='center'>
                            <Container text >
                                <Header className='cg-about-header-text'
                                    as='h1'
                                    content='Cognizant TaskCracker'
                                    inverted
                                    style={{
                                        marginTop: '2px',
                                    }}
                                />
                                <Header
                                    as='h2'
                                    content='Solve coding tasks and earn coins to win!'
                                    inverted
                                    style={{
                                        fontSize: '1.7em',
                                        fontWeight: 'normal',
                                        marginTop: '0',
                                    }}
                                />
                            </Container>
                        </Segment>
                    </div>
                </div>
                <Segment style={{ padding: '1em 0em 3em' }} vertical>
                    <Container style={{ paddingTop: '3em' }}>
                        <div className='cg-title'>
                            <h2>How to participate</h2>
                        </div>

                        <div className='cg-infographic-container'>
                            <div className='cg-infographic'>
                                <i className='lock icon cg-infographic-icon'></i>
                                <div className='cg-infographic-text-container'>
                                    <div className='cg-infographic-title'>Sign in</div>
                                    <div className='cg-infographic-text'>via Facebook or Google</div>
                                </div>
                            </div>
                            <div className='cg-infographic'>
                                <i className='laptop icon cg-infographic-icon'></i>
                                <div className='cg-infographic-text-container'>
                                    <div className='cg-infographic-title'>Solve</div>
                                    <div className='cg-infographic-text'>tasks and earn coins</div>
                                    <div className='cg-infographic-text-small'>individually or in a duel</div>    
                                </div>
                            </div>
                            <div className='cg-infographic'>
                                <i className='shopping cart icon cg-infographic-icon'></i>
                                <div className='cg-infographic-text-container'>
                                    <div className='cg-infographic-title'>Use</div>
                                    <div className='cg-infographic-text'>coins you have earned</div>
                                    <div className='cg-infographic-text-small'>buy at booth or save for big prizes</div>
                                </div>
                            </div>
                        </div>
                    </Container>
                    <Divider />
                    <Container style={{ paddingTop: '3em', paddingBottom: '3em' }}>
                        <div className='cg-title'>
                            <h2>3 ways to spend your coins</h2>
                        </div>
                        <div className='cg-about-p cg-points'>

                            <h3>Shop in the booth</h3>
                            <div>
                                <p className="margin-0">Coins can be spent in <a href={window.location.origin + "/shop"} target="_blank">virtual shopping booth</a> for different items. Contestants can purchase the same item only once</p>
                                <p className="margin-0">Spent coins are not added to your day or total balance</p>
                                <p className="margin-0">Coins can be spent in shopping booth only on the same day they are earned</p>
                                <p>Once an item has been purchased, given QR code acts as a receipt, which must be presented at Cognizant stand to redeem the prize</p>
                            </div>
                            
                            <h3>Save up for day prize</h3>
                            <div>
                                <p className="margin-0">At the end of each conference day, player of the day gets the <a href={window.location.origin + "/prizes"} target="_blank">day prize</a></p>
                                <p className="margin-0"><i>Wednesday: </i>the best duelist of the day wins Bluetooth JBL Headphones</p>
                                <p className="margin-0"><i>Thursday: </i>the player with most earned and unspent coins throughout Thursday wins Philips Electric Toothbrush</p>
                                <p><i>Friday: </i>the player with most earned and unspent points throughout Friday wins Sony Bluetooth Speaker</p>
                            </div>

                            <h3>Save up for conference prize</h3>
                            <div>
                                <p className="margin-0"><a href={window.location.origin + "/prizes"} target="_blank">GoPro Hero 5 Black</a> is awarded at the end of Friday to the leading contestant based on leaderboard results</p>
                                <p>In case of absence, the prize will go to the next best participant in the leaderboard</p>
                            </div>
                        </div>
                    </Container>

                    {/* <Divider />
                    <Container>
                        <div className='cg-title'>
                            <h2>Spending your coins</h2>
                        </div>
                        <p className='cg-about-p' style={{paddingBottom: 5}}>
                            Once you earn enough coins and want to purchase an item:
                        </p>
                        <div className='cg-about-p'>
                            <ul className='cg-ul-dash' style={{ paddingLeft: 25 }}>
                                <li>Go to virtual shopping booth</li>
                                <li>Select an item, click Buy
                                    <ul className='cg-ul-circle'>
                                        <li>Click on details to read more about the item</li>
                                        <li>Each registered contestant can purchase the same item only once</li>
                                    </ul>
                                </li>
                                <li>Receive a unique QR code (this is your purchase receipt)</li>
                                <li>Present the code to contest organizers to redeem the prize</li>
                            </ul>
                        </div>
                    </Container> */}

                    <Divider />
                    <Container style={{ paddingTop: '3em', paddingBottom: '3em' }}>
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
                                <li>The execution time of each solution must not exceed <b>5 seconds</b> and it must not consume more than <b>256 MB of RAM</b>. The solution's source file must not exceed <b>size of 256 KB</b></li>
                                <li>Challenge is active only during conference hours. The exact opening and closing time is shown at Cognizant stand</li>
                                <li>You can try to submit your solution as many times as you want. Once you submit a correct solution, you will gain the respective amount of coins and the task will be marked as solved</li>
                                <li>There is an editor provided near each of the tasks, but you can use anything you like – just make sure to put your code into the editor before you submit</li>
                                <li>Quantity of items in shopping booth is limited. Price of items may change throughout the conference</li>
                                <li>Coins, earned today, cannot be spend in shopping booth tomorrow. You can either save them up for conference prize or spend them the same day you earned them</li>
                                <li><b>Cheating is strictly prohibited!</b> The organizers remain the right to nullify a solution if it includes hacking</li>
                            </ul>
                        </div>
                    </Container>
                </Segment>
            </div>
        )
    }
}
