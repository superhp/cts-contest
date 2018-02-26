import * as React from 'react';
import { Form, Grid, Image, Transition } from 'semantic-ui-react';
import {
    Button,
    Container,
    Divider,
    Header,
    Icon,
    List,
    Menu,
    Segment,
    Visibility,
} from 'semantic-ui-react';
import { RouteComponentProps } from 'react-router';

import q from './Graph'; 

export default class Questions extends React.Component<any, any> {
    state = { question: q.curr, animation: "fade right", duration: 1000, visible: true, lastAnswer: '' }

    handleAnswerSelection = (answerText: string) => {
        
        console.log("kabutes");
        this.setState({ visible: false, lastAnswer: answerText });
       // this.setState({ question: q.curr, visible: true });
    };

    showNextQuestion = () => {
        console.log("damutes");
        q.answer(this.state.lastAnswer);
        this.setState({ question: q.curr, visible: true });
    }

    render() {
        const { animation, duration, visible } = this.state;
        console.log(visible);
        return (
                <Segment style={{ padding: '1em 0em 3em' }} vertical>
                    <Container>
                        <Transition visible={this.state.visible} animation='horizontal flip' duration={500} onHide={() => this.showNextQuestion()} transitionOnMount={true}>
                            <div className='cg-card'>
                                <div style={{ position: 'absolute', left: 0, top: 0, width: '100%', zIndex: 1 }}>
                                    <div className='cg-title'>
                                        <h2>{this.state.question.text}</h2>
                                    </div>
                                </div>
                                <div className='cg-card-content'>
                                    <div className='cg-card-actions' style={{ marginTop: 71}}>
                                        {this.state.question.edges.map(x =>
                                            <div className='cg-action-item' key={x.text} >
                                                <button className='cg-card-button cyan' onClick={() => this.handleAnswerSelection(x.text)} > {x.text}</button>
                                            </div>
                                        )}
                                    </div>
                                </div>
                            </div>
                        </Transition>
                    </Container>
                </Segment>
        );
    }
}



