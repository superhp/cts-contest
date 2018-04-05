import React from 'react';
import { Menu } from 'semantic-ui-react';
import Timer from './Timer.jsx';

export default class Header extends React.Component {
    constructor(props) {
        super(props);
    }
    renderTimer() {
        switch (this.props.board) {
            case 0:
                return <Timer day={this.props.timer.dayGame.day} hour={this.props.timer.dayGame.hour} minutes={this.props.timer.dayGame.minute} />;
            case 1:
                return <Timer day={this.props.timer.dayGame.day} hour={this.props.timer.dayGame.hour} minutes={this.props.timer.dayGame.minute} onlyWarning />;
            case 2:
                return <Timer day={this.props.timer.dayGame.day} hour={this.props.timer.dayGame.hour} minutes={this.props.timer.dayGame.minute} />;
            case 3:
                return <Timer day={this.props.timer.conferenceGame.day} hour={this.props.timer.conferenceGame.hour} minutes={this.props.timer.conferenceGame.minute} />;
            default:
                return null;
        }
    }
    render() {

        return (
            <Menu className="cg-nav" size='large' stackable color='blue' inverted>
                <Menu.Item className='cg-nav-header' header>
                    <img className='cg-nav-logo' src="./img/Cognizant_LOGO.png" alt="Cognizant logo"/>
                </Menu.Item>
                <Menu.Menu position='right'>
                    <div className='cg-title'>
                        www.cognizantchallenge.lt
                    </div>
                    <div className='cg-timeleft'>
                        {this.renderTimer()}
                    </div>

                </Menu.Menu>
            </Menu>
        );
    }

}