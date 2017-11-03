﻿import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Card, Icon, Image, Grid, Button, Label } from 'semantic-ui-react';

export class ShopItemCard extends React.Component<any, any> {
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
                        </div><div className='cg-action-item'>
                            {this.props.userLogedIn
                                ? this.renderButtons()
                                : this.renderEmpty()
                            }
                        </div>


                    </div>
                </div>
            </div>
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
                <button
                    className='cg-card-button cyan'
                    onClick={this.openPurchaseQR}
                >QR</button>
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