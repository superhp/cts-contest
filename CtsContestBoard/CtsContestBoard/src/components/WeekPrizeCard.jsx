import React from 'react';
import { Card, Label, Image } from 'semantic-ui-react';

export default class PrizeCard extends React.Component {
    render() {
        
        return (
            <Card className={this.props.className} fluid>
                {/* <Label className="card-ribbon" color='green' ribbon>SPECIAL PRIZE!</Label> */}
                <Image src={this.props.picture}/>
                <Card.Content className="card-daily-prize-header">
                    <h2 className="prizeHeader">
                        {this.props.name}
                    </h2>
                </Card.Content>
            </Card>
        )
    }

}