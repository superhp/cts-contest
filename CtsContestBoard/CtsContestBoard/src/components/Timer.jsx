import React from 'react';
import { Menu } from 'semantic-ui-react';

export default class Timer extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            days: 0,
            hours: 0,
            minutes: 0,
            seconds: 0
        }
        this.getTimeToEnd = this.getTimeToEnd.bind(this);

        this.renderNormal = this.renderNormal.bind(this);
        this.renderWarning = this.renderWarning.bind(this);
        this.renderTimeOut = this.renderTimeOut.bind(this);
    }
    componentDidMount() {
        this.endTime = new Date(
            2017,
            11 - 1,
            this.props.day,
            this.props.hour,
            this.props.minutes,
            0
        );
        this.loadInterval();
    }
    componentWillReceiveProps(nextProps) {
        this.endTime = new Date(
            2017,
            11 - 1,
            nextProps.day,
            nextProps.hour,
            nextProps.minutes,
            0
        );
        this.loadInterval();
    }
    componentWillUnmount() {
        clearInterval(this.leftInterval);
    }
    loadInterval() {
        this.getTimeToEnd();
        if (this.leftInterval !== undefined) {
            clearInterval(this.leftInterval);
        }
        this.leftInterval = setInterval(this.getTimeToEnd, 1000);
    }
    getTimeToEnd() {
        const currentTime = new Date();
        const endTime = this.endTime;

        const distance = endTime - currentTime;

        this.setState({
            days: Math.floor(distance / (1000 * 60 * 60 * 24)),
            hours: Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)),
            minutes: Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60)),
            seconds: Math.floor((distance % (1000 * 60)) / 1000)
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
        if (this.state.days <= 0 && this.state.hours <= 0 && this.state.minutes <= 0 && this.state.seconds <= 0) {
            //clearInterval(this.leftInterval);
            return this.renderTimeOut();
        }
        if (this.state.days <= 0 && this.state.hours <= 0) {
            if (this.props.onlyWarning)
                return this.renderShopTimer();
            return this.renderWarning();
        }
        if (this.props.onlyWarning)
            return  <p className='cg-timer warning shop'/>
        return this.renderNormal()
    }
    renderNormal() {
        return (
            <p className='cg-timer' style={this.props.style}>
                {this.state.days === 0
                    ? ''
                    : this.state.days === 1
                        ? this.state.days + ' Day, '
                        : this.state.days + ' Days, '}
                {this.parseTimeLeft(this.state.hours, this.state.minutes, this.state.seconds)}
            </p>
        )
    }
    renderWarning() {
        return (
            <p className='cg-timer warning' style={this.props.style}>
                {this.state.days === 0
                    ? ''
                    : this.state.days === 1
                        ? this.state.days + ' Day, '
                        : this.state.days + ' Days, '}
                {this.parseTimeLeft(this.state.hours, this.state.minutes, this.state.seconds)}
            </p>
        )
    }
    renderTimeOut() {
        return (
            <p className='cg-timer time-out' style={this.props.style}>
                Game over
            </p>
        )
    }
    renderShopTimer() {
        return (
            <p className='cg-timer warning shop' style={this.props.style}>
                Shop will close in {this.parseTimeLeft(this.state.hours, this.state.minutes, this.state.seconds)}
            </p>
        )
    }

}