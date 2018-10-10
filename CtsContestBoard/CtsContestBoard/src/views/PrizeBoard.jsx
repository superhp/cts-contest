import React from 'react';
import PrizeCard from '../components/PrizeCard.jsx';
import Timer from '../components/Timer.jsx';
export default class PrizeBoard extends React.Component {
    constructor(props) {
        super(props);
        //console.log(props);
    }
    render() {
        const prizes = [...this.props.prizes];
        // for (let i = 0; i <4; i++) {
        //     prizes.push({ Id:1302,Name:"Ausines",Picture:"http://cts-contest-cms.azurewebsites.net/media/1089/prizas-1-pele-logitech-684x1024.jpg",Price:3,Quantity:0});
        // }
        return (
            <div className="container">
                {/*<Timer day={new Date().getDate()} hour={17} minutes={0} onlyWarning />*/}
                <div className='prize-list' style={{ paddingTop: 40 }}>
                    {prizes.map((prize, index) =>
                        <div className='col' key={index}>
                            <PrizeCard
                                prize={prize}
                            />
                        </div>
                    )}
                </div>
            </div>
        );
    }

}