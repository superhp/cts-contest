import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Card, Icon, Image, Grid, Button } from 'semantic-ui-react';

interface PrizeCardState {
}
export class PrizeCard extends React.Component<PrizeCardProps, PrizeCardState> {

    buy = () => {
        this.props.onBuy(this.props.id);
    }

    public render() {
        return <Card color={this.props.purchased ? 'green' : 'blue'}>
            <Image src={this.props.picture}>

            </Image>
            <Card.Content>
                <Card.Header>
                    {this.props.name}
                </Card.Header>
            </Card.Content>
            <Card.Content extra>
                <div className='row'>
                    <div className='col-xs-6 col-center-block'>
                        <p>{this.props.price}&nbsp;x&nbsp;
                                <Icon name='money' /></p>
                        <p>{this.props.quantity} left</p>
                    </div>
                    <div className='col-xs-6 col-center-block' style={{ paddingLeft: 0 }}>
                        {this.props.purchased
                            ? <div style={{float: 'right'}}><Icon name='check circle outline' color='green' size='huge'/></div>
                            : <Button
                                inverted
                                color='blue'
                                size='big'
                                fluid
                                onClick={this.buy}
                                disabled={this.props.quantity <= 0 || this.props.price > this.props.balance ? true : false}
                            >Buy</Button>}

                    </div>
                </div>
            </Card.Content>
        </Card>
    }
}
/*export interface Prize {
    id: number;
    price: number;
    quantity: number;
    name: string;
    picture: string;
}
interface PrizeCardProps {
    prize: Prize;
}*/
interface PrizeCardProps {
    id: number;
    price: number;
    quantity: number;
    name: string;
    picture: string;
    onBuy: any;
    balance: number;
    purchased?: boolean;
}