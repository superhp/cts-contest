import React from 'react';

const GameOver = (props) => {
    return <p className='cg-timer time-out warning-timer' style={props.style}>
            Game Over
        </p>
}

const TimeLeft = (props) => {
    return <div>
            {props.infoText}
            <p className={'cg-timer ' + props.warningClass} style={props.style}>
                {props.timeLeft}
            </p>
        </div>
}

export default class Timer extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            days: 0,
            hours: 0,
            minutes: 0,
            seconds: 0,
        }
        this.getTimeToEnd = this.getTimeToEnd.bind(this);
    }

    componentDidMount() {
        this.onReceivedProps(this.props.day, this.props.hour, this.props.minutes);
        this.loadInterval();
    }

    componentWillReceiveProps(nextProps) {
        this.onReceivedProps(nextProps.day, nextProps.hour, nextProps.minutes);
        this.loadInterval();
    }
    
    componentWillUnmount() {
        clearInterval(this.leftInterval);
    }

    onReceivedProps(day, hour, minutes) {
        this.endTime = new Date(
            2018,
            9 - 1,
            day,
            hour,
            minutes,
            0
        );
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
        const distance = this.endTime - currentTime;

        this.setState({
            days: Math.floor(distance / (1000 * 60 * 60 * 24)),
            hours: Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)),
            minutes: Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60)),
            seconds: Math.floor((distance % (1000 * 60)) / 1000)
        });
    }

    parseTimeLeft(hours, minutes, seconds) {
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
        let gameEnded = ((this.state.days <= 0 && this.state.hours <= 0 && this.state.minutes <= 0) || this.state.hours === 23);
        if (gameEnded) {
            return <GameOver style={this.props.style}/>
        }
        
        let timeLeft = this.parseTimeLeft(this.state.hours, this.state.minutes, this.state.seconds);
        let warningClass = this.state.hours === 0 && this.state.minutes <= 15 ? 'warning-timer' : null;
        return <TimeLeft style={this.props.style} timeLeft={timeLeft} infoText={this.props.informationViewText} parseTimeLeft={this.state.parseTimeLeft}
            warningClass={warningClass}/>
    }
}