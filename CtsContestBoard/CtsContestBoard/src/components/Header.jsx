import React from 'react';
import { Menu } from 'semantic-ui-react';

const endHours = 17;
const endMinutes = 0;

export default class Header extends React.Component {
    constructor(props) {
        super(props);


    }

    render() {

        return (
            <Menu className="cg-nav" size='large' stackable color='blue' inverted>
                <Menu.Item className='cg-nav-header' header>
                    <img className='cg-nav-logo' src="./img/logo.svg" alt="Cognizant logo" />
                </Menu.Item>
                {/* <Menu.Menu position='right'>
                    <div className='cg-timeleft'>
                        <span style={{paddingRight: 20}}>Time left: </span>{this.parseTimeLeft(this.state.timeLeft.hours, this.state.timeLeft.minutes, this.state.timeLeft.seconds)}
                    </div>

                </Menu.Menu> */}
            </Menu>
        )
    }

}