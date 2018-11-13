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
        let informationViewText = <h1 style={{ fontSize: 72 }}>Game stops in:</h1>;
        return (
            <div className="container">
                <div style={{ textAlign: 'center', paddingTop: 100, paddingBottom: 0, margin: 0 }}>
                    <Timer day={this.props.timer.dayGame.day} hour={this.props.timer.dayGame.hour} minutes={this.props.timer.dayGame.minute}
                        style={{ color: 'black', fontSize: 72}} informationViewText={informationViewText}/>
                </div>
                <p style={{ margin: 0, fontSize: 60, textAlign: 'center', paddingTop: 40,paddingBottom: 10, fontWeight: 'bold' }}>
                    Conference prize winner will be awarded on Friday at {this.renderTimeValue(this.props.timer.conferenceGame.hour)}:{this.renderTimeValue(this.props.timer.conferenceGame.minute)}        
                </p>
                <p style={{ margin: 0, fontSize: 60, textAlign: 'center', paddingTop: 40,paddingBottom: 10, fontWeight: 'bold' }}>
                    Items in shopping booth can be purchased and withdrawn until {this.renderTimeValue(this.props.timer.dayGame.hour)}:{this.renderTimeValue(this.props.timer.dayGame.minute)}        
                </p>                
                 
                <p style={{ margin: 0, fontSize: 60, textAlign: 'center' }}>
                    Please come to Cognizant stand to take your prize
                </p>
                <p style={{margin: 0, fontSize: 60, textAlign: 'center' }}>
                    Game will be stopped today at 18:00
                </p>
                <p style={{ margin: 0, fontSize: 60, textAlign: 'center' }}>
                    Today's balance will reset and will only be valid for Conference prize.
                </p>*
            </div>
        );
    }
}