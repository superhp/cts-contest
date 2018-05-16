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
                                <li>Login using details we sent to your VGTU email</li>
                                <li>Solve coding tasks to earn points
                                    <ul className='cg-ul-dash' style={{paddingLeft: 25}}>
                                        <li>Tasks are divided into several groups based on their complexity</li>
                                        <li>Each task can be solved only once</li>
                                        <li>You can earn 10-645 points for each correct solution, depending on task’s difficulty</li>
                                    </ul>
                                </li>
                                <li>Have points added to your virtual wallet</li>
                            </ol>
                        </div>
                    </Container>

                    <Divider />
                    <Container>
                        <div className='cg-title'>
                            <h2>Other rules</h2>
                        </div>
                        <div className='cg-about-p'>
                            <ul className='cg-ul-dash' style={{ paddingLeft: 25 }}>
                                <li>You can code using C++ programming language.</li>
                                <li>The execution time of each solution must not exceed 5 seconds and it must not consume more than 256 MB of memory. The size of the source file of a solution must not exceed 256 KB.</li>
                                <li>There is an editor provided near each of the tasks, but you can use anything you like – just make sure to put your code into the editor before you submit.</li>
                                <li>You can try to submit your solution as many times as you want. Once your code passes the tests, you will gain the respective amount of points and the task will be marked as solved.</li>
                            </ul>
                        </div>
                    </Container>
                </Segment>
            </div>
        )
    }
}
