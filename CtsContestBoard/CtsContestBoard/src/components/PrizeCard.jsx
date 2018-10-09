import React from 'react';
import { Card, Image, Segment, Label } from 'semantic-ui-react';

export default class PrizeCard extends React.Component {

    render() {
        let label = label = <div className='cardFooter'>{this.props.prize.Quantity} left</div>;
        if (this.props.prize.Quantity <= 3 && this.props.prize.Quantity > 0) label = <div className='cardFooter yellow'>ONLY {this.props.prize.Quantity} LEFT!</div>;
        if (this.props.prize.Quantity === 0) label = <div className='cardFooter sold-out'>SOLD OUT</div>;
        // return (
        //     <Card className="prizeCard">

        //         <Image src={this.props.prize.Picture} /*label={{as: 'a', conent: }}*/ />
        //         <div className="cardHeader">
        //             <p className="headerText">
        //                 {this.props.prize.Name}
        //             </p>
        //         </div>
        //         {label}
        //     </Card>
        // )
        return (
            
            <div className='prize-card'>
                {label}
                <div className='img-container'>
                    <img className='center-image' src={this.props.prize.Picture} alt=""/>
                </div>
                <p className='prize-card-header'>
                    {this.props.prize.Name}
                </p>
                
            </div>
        )
    }

}