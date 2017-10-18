import React from 'react';
import { Card, Label, Image } from 'semantic-ui-react';

export default class PrizeCard extends React.Component {
    render() {
        
        return (
            <Card className={this.props.className} fluid>
                <Label className="card-ribbon" color='green' ribbon>SPECIAL PRIZE!</Label>
                <Image src={this.props.picture} alt='Special prize'/>
                <Card.Content className="cardHeader">
                    <Card.Header className="headerText">
                        {this.props.name}
                    </Card.Header>
                </Card.Content>
            </Card>
        )
    }

}