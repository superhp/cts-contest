import React from 'react';
import { Menu } from 'semantic-ui-react';
import Timer from './Timer.jsx';
import { BoardEnum } from '../Board.jsx';

export default class Header extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <Menu className="cg-nav" size='large' stackable color='blue' inverted>
                <Menu.Item className='cg-nav-header' header>
                    <img className='cg-nav-logo' src="./img/CognizantLogo.svg" alt="Cognizant logo"/>
                </Menu.Item>
                <Menu.Menu position='right'>
                    <div className='cg-title'>
                        www.cognizantchallenge.lt
                    </div>
                    <div className='cg-timeleft'>
                        <Timer day={this.props.timer.dayGame.day} hour={this.props.timer.dayGame.hour} minutes={this.props.timer.dayGame.minute}
                            hiddenInformationHeader={this.props.board === BoardEnum.Information ? true : null} />
                    </div>

                </Menu.Menu>
            </Menu>
        );
    }

}