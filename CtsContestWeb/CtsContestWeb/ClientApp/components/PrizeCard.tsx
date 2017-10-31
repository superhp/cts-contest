import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Card, Icon, Image, Grid, Button, Label } from 'semantic-ui-react';

interface PrizeCardState {
}
export class PrizeCard extends React.Component<any, any> {
    constructor(props: any) {
        super(props);
    }
    buy = () => {
        this.props.onBuy(this.props.prize.id);
    }

    openPurchaseQR = () => {
        this.props.onOpenPurchaseQR(this.props.prize);
    }
    openDescription = () => {
        this.props.onOpenDescription(this.props.prize);
    }

    public render() {
        return (
            <div className='cg-card'>
                <div className='cg-card-label'>
                    <div className='left'>
                        Price:  {this.props.prize.price}
                    </div>
                    <div className='right'>
                        <p className='cg-prize-quantity'>
                            {this.props.prize.quantity}
                        </p>
                        <p className='cg-prize-quantity-after'>
                            Left
                        </p>
                    </div>
                </div>
                <div className='cg-card-image'>
                    <img src={this.props.prize.picture} alt="" />
                </div>
                <div className='cg-card-content'>
                    <h2 className='cg-card-title'>{this.props.prize.name}</h2>
                    <div className='cg-card-actions'>
                        <div className='cg-action-item'>
                            <button className='cg-card-button cyan' onClick={this.openDescription}>Details</button>
                        </div>
                        <div className='cg-action-item'>
                            {this.props.userLogedIn
                                ? this.renderButtons()
                                : this.renderEmpty()
                            }
                        </div>
                    </div>
                </div>
            </div>
        )
        // return (
        //     <Card className='cg-card' color={this.props.purchased ? 'green' : 'blue'}>
        //         <Image src={this.props.prize.picture}
        //             fluid
        //             label={this.props.prize.quantity <= 0
        //                 ? { as: 'div', color: 'red', content: 'Sold out', ribbon: 'right' }
        //                 : {
        //                     as: 'div',
        //                     color: 'blue',
        //                     content: <p>{this.props.prize.price}&nbsp;<Icon name='money' /></p>,
        //                     ribbon: 'right'
        //                 }
        //             }
        //         />

        //         <Card.Content>
        //             <Card.Header>
        //                 {this.props.prize.name}
        //             </Card.Header>
        //         </Card.Content>
        //         <Card.Content extra>
        //             <div className='row'>
        //                 <div className='col-xs-6 col-center-block'>
        //                     <p>{this.props.prize.price}&nbsp;x&nbsp;
        //                         <Icon name='money' /></p>
        //                     <p>{this.props.prize.quantity} left</p>
        //                 </div>
        //                 <div className='col-xs-6 col-center-block' style={{ paddingLeft: 0 }}>
        //                     {this.props.userLogedIn
        //                         ? this.renderButtons()
        //                         : this.renderEmpty()
        //                     }
        //                 </div>
        //             </div>
        //         </Card.Content>
        //     </Card>
        // )
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
                <button
                    className='cg-card-button cyan'
                    onClick={this.openPurchaseQR}
                >Details</button>
            )
        }
        else
            return (
                <button
                    className='cg-card-button secondary'
                    disabled={this.props.prize.quantity <= 0 || this.props.prize.price > this.props.balance ? true : false}
                    onClick={this.buy}
                >Buy</button>
            )
        // return (
        //     <Button
        //         className='cg-button cg-button-buy'
        //         onClick={this.buy}
        //         disabled={this.props.prize.quantity <= 0 || this.props.prize.price > this.props.balance ? true : false}
        //     >Buy</Button>
        // )
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