import React from 'react';
import { Menu } from 'semantic-ui-react';

const endHours = 17;
const endMinutes = 0;

export default class Header extends React.Component {
    constructor(props) {
        super(props);


        this.state = {
            timeLeft: {
                hours: 0,
                minutes: 0,
                seconds: 0
            }
        }
        this.getTimeToEnd = this.getTimeToEnd.bind(this);
    }
    componentDidMount() {
        this.getTimeToEnd();
        if (this.leftInterval === undefined) {
            this.leftInterval = setInterval(this.getTimeToEnd, 1000);
        }
    }
    getTimeToEnd() {
        const currentTime = new Date();
        const endTime = new Date();
        endTime.setHours(endHours);
        endTime.setMinutes(endMinutes);
        endTime.setSeconds(0);

        const distance = endTime - currentTime;

        this.setState({
            timeLeft: {
                hours: Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)),
                minutes: Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60)),
                seconds: Math.floor((distance % (1000 * 60)) / 1000)
            }
        });

    }
    parseTimeLeft(hours, minutes, seconds) {
        if (hours <= 0 && minutes <= 0 && seconds <= 0) {
            //clearInterval(this.leftInterval);
            return 'Game over';
        }
        let timeString = '';


        if (hours < 10) timeString += '0' + hours + ' : ';
        else timeString += hours + ' : ';

        if (minutes < 10) timeString += '0' + minutes + ' : ';
        else timeString += minutes + ' : ';

        if (seconds < 10) timeString += '0' + seconds;
        else timeString += seconds;

        return timeString;
    }

    render() {

        return (
            <Menu className="cg-nav" size='large' stackable color='blue' inverted>
                <Menu.Item className='cg-nav-header' header>
                    <img className='cg-nav-logo' src="./img/logo.svg" alt="Cognizant logo" />
                </Menu.Item>
                <Menu.Menu position='right'>
                    <div className='cg-timeleft'>
                        <span style={{paddingRight: 20}}>Time left: </span>{this.parseTimeLeft(this.state.timeLeft.hours, this.state.timeLeft.minutes, this.state.timeLeft.seconds)}
                    </div>

                </Menu.Menu>
            </Menu>
        )
    }

}