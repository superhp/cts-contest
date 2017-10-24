import React from 'react';
import PrizeCard from '../components/PrizeCard.jsx';

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
                {prizes.map((prize, index) =>
                    <div className='col-prize' key={index} style={{ paddingBottom: 20 }}>
                        <PrizeCard
                            prize={prize}
                        />
                    </div>
                )}
            </div>
        );
    }

}