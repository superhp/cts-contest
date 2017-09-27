import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Grid } from 'semantic-ui-react';
//https://react.semantic-ui.com/usage stylesheet missing
import { PrizeModal } from './PrizeModal';
import { PurchaseModal } from './PurchaseModal';
import { PrizeCard } from './PrizeCard';
import 'isomorphic-fetch';



interface PrizesState {
    prizes: Prize[];
    loading: boolean;

    prizeModalOpen: boolean;
    prizeModalData: Prize;

    purchaseModalOpen: boolean;
    purchaseModalState: string;
    purchaseId: string;
}

export class Prizes extends React.Component<RouteComponentProps<{}>, PrizesState> {
    constructor() {
        super();
        this.state = {
            prizes: [], loading: true,
            prizeModalOpen: false,

            purchaseModalState: 'closed',
            purchaseModalOpen: false,
            purchaseId: "",

            prizeModalData: {
                id: 0,
                name: "",
                price: 0,
                quantity: 0,
                picture: ""
            }
        };

        fetch('api/Prize')
            .then(response => response.json() as Promise<Prize[]>)
            .then(data => {
                this.setState({ prizes: data, loading: false });
            });
    }
    buy = (prize: Prize) => {
        fetch('api/Purchase/Buy', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                userEmail: 'user@gmail.com',
                prizeId: prize.id,
            })
        })
            .then(response => response.json() as Promise<Purchase>)
            .then(data => {
                this.setState({ purchaseId: data.id, purchaseModalState: 'loaded' });
            }).catch(error => {
                //console.log(error);
                this.setState({ purchaseId: prize.name, purchaseModalState: 'error' });
            });
        this.openPurchaseModal(prize);
    }

    /*
     * QR modal
     */
    openPurchaseModal = (prize: Prize) => {
        this.setState({ purchaseModalOpen: true, purchaseModalState: 'loading' });
    }

    closePurchaseModal = () => {
        this.setState({ purchaseModalOpen: false, purchaseModalState: 'closed' });
    }

    /* 
     * 'Are your sure' modal
     */
    openPrizeModal = (id: number) => {
        const prize = this.state.prizes.filter((pr: Prize) => { return pr.id === id })[0];
        this.setState({ prizeModalData: prize, prizeModalOpen: true});
    }

    closePrizeModal = () => {
        this.setState({
            prizeModalOpen: false,
        });
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderForecastsTable(this.state.prizes);

        return <div>
            <h1>Prizes</h1>
            <p>Buy some stuff!</p>
            {contents}
            <PrizeModal
                open={this.state.prizeModalOpen}
                onClose={this.closePrizeModal}
                prize={this.state.prizeModalData}
                onBuy={this.buy} />
            <PurchaseModal
                open={this.state.purchaseModalOpen}
                onClose={this.closePurchaseModal}
                prize={this.state.prizeModalData}
                state={this.state.purchaseModalState}
                purchaseId={this.state.purchaseId} />
        </div>;
    }

    private renderForecastsTable(prizes: Prize[]) {
        var groups = new Array<Array<Prize>>();
        prizes.forEach((val, i) => {
            var idx = Math.floor(i / 4);
            if (groups.length <= idx) {
                groups.push(new Array<Prize>());
            }
            groups[idx].push(val)
        });
        return <Grid columns={4}>
            {groups.map((group, rowIndex) =>
                <Grid.Row key={rowIndex}>
                    {group.map((prize, colIndex) =>
                        <Grid.Column key={colIndex}>
                            <PrizeCard id={prize.id} name={prize.name} picture={prize.picture} quantity={prize.quantity} price={prize.price} onBuy={this.openPrizeModal} />
                        </Grid.Column>
                    )}
                </Grid.Row>
            )}
        </Grid>;
    }
}

interface Prize {
    id: number;
    price: number;
    quantity: number;
    name: string;
    picture: string;
}

interface Purchase {
    id: string;
}