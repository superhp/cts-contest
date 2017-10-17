import React from 'react';
import { Card, Image, Segment, Label } from 'semantic-ui-react';

export default class PrizeCard extends React.Component {
 
    render() {
        let label = label = <h3><p >{this.props.prize.Quantity} left</p></h3>; 
        if (this.props.prize.Quantity <= 3 && this.props.prize.Quantity > 0) label = <Label color='yellow' ribbon>ONLY {this.props.prize.Quantity} LEFT!</Label>;
        if (this.props.prize.Quantity === 0) label = <Label color='red' ribbon>SOLD OUT</Label>;
        return (
            <Card className="prizeCard">
                    
                    <Image src={this.props.prize.Picture} />
                    <Card.Content className="cardHeader">
                    <Card.Header className="headerText">
                            {this.props.prize.Name}
                        </Card.Header>
                    </Card.Content>
                    <Card.Content>
                    
                        <div className='row'>
                            <div className='col-xs-6 col-center-block'>
                           
                            {label}
                               
                            </div>
                        </div>
                    </Card.Content>
            </Card>
        )
    }

}