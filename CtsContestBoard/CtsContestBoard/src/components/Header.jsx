import React from 'react';
import { Menu } from 'semantic-ui-react';
import Timer from './Timer.jsx';

//Days timer
const endHours = 16;
const endMinutes = 0;

//Conference timer
const confEndDay = 17;
const confEndHours = 16;
const confEndMinutes = 0;

export default class Header extends React.Component {
    constructor(props) {
        super(props);


    }
    renderTimer() {
        switch (this.props.board) {
            case 0:
                return null;
            case 1:
                return <Timer day={new Date().getDate()} hour={endHours} minutes={endMinutes} onlyWarning />;
            case 2:
                return <Timer day={new Date().getDate()} hour={endHours} minutes={endMinutes} />;
            case 3:
                return <Timer day={confEndDay} hour={confEndHours} minutes={confEndMinutes} />;
        }
    }
    render() {

        return (
            <Menu className="cg-nav" size='large' stackable color='blue' inverted>
                <Menu.Item className='cg-nav-header' header>
                    <img className='cg-nav-logo' src="./img/logo.svg" alt="Cognizant logo" />
                </Menu.Item>
                 <Menu.Menu position='right'>
                    <div className='cg-timeleft'>
                        {this.renderTimer()}
                    </div>

                </Menu.Menu>
            </Menu>
        )
    }

}