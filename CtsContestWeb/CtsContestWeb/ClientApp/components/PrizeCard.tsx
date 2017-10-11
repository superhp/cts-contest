import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Card, Icon, Image, Grid, Button, Label } from 'semantic-ui-react';

interface PrizeCardState {
}
export class PrizeCard extends React.Component<any, PrizeCardState> {

    buy = () => {
        this.props.onBuy(this.props.prize.id);
    }

    openPurchaseQR = () => {
        this.props.onOpenPurchaseQR(this.props.prize);
    }

    public render() {
        return (
            <Card className='cg-card' color={this.props.purchased ? 'green' : 'blue'}>
                <Image src={this.props.prize.picture}
                    fluid
                    label={this.props.prize.quantity <= 0
                        ? { as: 'div', color: 'red', content: 'Sold out', ribbon: 'right' }
                        : {
                            as: 'div',
                            color: 'blue',
                            content: <p>{this.props.prize.price}&nbsp;<Icon name='money' /></p>,
                            ribbon: 'right'
                        }
                    }
                />

                <Card.Content>
                    <Card.Header>
                        {this.props.prize.name}
                    </Card.Header>
                </Card.Content>
                <Card.Content extra>
                    <div className='row'>
                        <div className='col-xs-6 col-center-block'>
                            <p>{this.props.prize.price}&nbsp;x&nbsp;
                                <Icon name='money' /></p>
                            <p>{this.props.prize.quantity} left</p>
                        </div>
                        <div className='col-xs-6 col-center-block' style={{ paddingLeft: 0 }}>
                            {this.props.userLogedIn
                                ? this.renderButtons()
                                : this.renderEmpty()
                            }
                        </div>
                    </div>
                </Card.Content>
            </Card>
        )
    }
    renderEmpty() {
        return '';
    }
    renderButtons() {

        if (this.props.purchased) {
            if (this.props.purchase.isGivenAway) {
                return (
                    <div>Item received</div>
                )
            }
            return (
                <Button
                    className='cg-button cg-button-qr'
                    onClick={this.openPurchaseQR}
                >QR</Button>
            )
        }
        else
            return (
                <Button
                    className='cg-button cg-button-buy'
                    onClick={this.buy}
                    disabled={this.props.prize.quantity <= 0 || this.props.prize.price > this.props.balance ? true : false}
                >Buy</Button>
            )
    }
}
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