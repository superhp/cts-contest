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
import DataForm from './DataForm';
import { NodeType } from './Graph';

export default class Questions extends React.Component<any, any> {
    state = { question: q.curr, animation: "fade right", duration: 1000, visible: true, lastAnswer: '' }

    handleAnswerSelection = (answerText: string) => {
        this.setState({ visible: false, lastAnswer: answerText });
       // this.setState({ question: q.curr, visible: true });
    };

    showNextQuestion = () => {
        q.answer(this.state.lastAnswer);
        this.setState({ question: q.curr, visible: true });
    }

    render() {
        const { animation, duration, visible } = this.state;
        return (
                <Segment style={{ padding: '1em 0em 3em' }} vertical>
                    <Container>
                        <Transition visible={this.state.visible} animation='horizontal flip' duration={500} onHide={() => this.showNextQuestion()} transitionOnMount={true}>
                            <div className='cg-card questions'>
                                <div style={{  width: '100%', zIndex: 1 }}>
                                    <div className='cg-title questionAlign'>
                                        <h2 dangerouslySetInnerHTML={ { __html: this.state.question.text } } ></h2>
                                    </div>
                                    {this.state.question.type === NodeType.A ? <div><Divider className="divider" /> <DataForm /></div> : ""}
                                </div>
                                <div className='cg-card-content'>
                                    <div className='cg-card-actions'>
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



