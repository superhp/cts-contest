import React from 'react';
import PrizeCard from '../components/PrizeCard.jsx';
import Timer from '../components/Timer.jsx';
export default class Information extends React.Component {
    constructor(props) {
        super(props);
    }
    renderTimeValue(value) {
        if (value < 10)
            return '0' + value;
        else
            return value;
    }
    render() {
        let leftTime = this.props.timer.dayGame.minute !== 0
            ? <h1 style={{ fontSize: 72, color: '#4298b5' }}>Game closes in:</h1>
            : <div />;
        return (
            <div className="container">
                <div style={{ textAlign: 'center', paddingTop: 25, paddingBottom: 0, margin: 0 }}>
                    {leftTime}
                    <Timer day={this.props.timer.dayGame.day} hour={this.props.timer.dayGame.hour} minutes={this.props.timer.dayGame.minute} style={{ color: 'black', fontSize: 72}}/>
                </div>                

                <p style={{ margin: 0, fontSize: 60, textAlign: 'center', paddingTop: 40,paddingBottom: 10, fontWeight: 'bold' }}>
                    The winner will be awarded at {this.renderTimeValue(this.props.timer.dayGame.hour)}:{this.renderTimeValue(this.props.timer.dayGame.minute)}        
                </p>

                <p style={{ margin: 0, fontSize: 60, textAlign: 'center' }}>
                    Please come to Cognizant stand to take daily prize.
                </p>

                <p style={{ margin: 0, fontSize: 60, textAlign: 'center' }}>
                    Today's balance will reset and will only be valid for Conference prize.
                </p>
                
                <p style={{ margin: 0, fontSize: 60, textAlign: 'center' }}>
                    Please withdraw your purchases until this time.
                </p>

                <p style={{margin: 0, fontSize: 60, textAlign: 'center' }}>
                     After this time you will not be able submit new solutions.
                </p>         
            </div>
        );
    }
}