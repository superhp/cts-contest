import React from 'react';
import { Card, Image } from 'semantic-ui-react';

export default class PrizeCard extends React.Component {
    constructor(props) {
        super(props);
        console.log(props);
    }
    render() {
        return (
            <Card>
                <Image src={this.props.prize.Picture} />
                <Card.Content>
                    <Card.Header>
                        {this.props.prize.Name}
                    </Card.Header>
                </Card.Content>
                <Card.Content extra>
                    
                    <div className='row'>
                        <div className='col-xs-6 col-center-block'>
                            <p>{this.props.prize.Quantity} left</p>
                        </div>
                    </div>
                </Card.Content>
            </Card>
        )
    }

}