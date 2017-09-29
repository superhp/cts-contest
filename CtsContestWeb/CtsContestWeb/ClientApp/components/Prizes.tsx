import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Grid, Container, Header, Icon, Loader, Divider } from 'semantic-ui-react';
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

export class Prizes extends React.Component<any, any> {
    constructor() {
        super();
        this.state = {
            prizes: [], loading: true,
            prizeModalOpen: false,

            purchaseModalState: 'closed',
            purchaseModalOpen: false,
            purchaseId: "",
            purchasedItems: {},

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
        fetch('api/Purchase/Purchase', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                prizeId: prize.id,
            })
        })
            .then(response => response.json() as Promise<Purchase>)
            .then(data => {
                this.handleBuyResponse(prize, data);
            }).catch(error => {
                console.log(error);
                this.setState({ purchaseId: "", purchaseModalState: 'error' });
            });
        this.openPurchaseModal(prize);
    }

    handleBuyResponse(prize: any, data: any) {
        if (Object.keys(data)[0] === 'error') {
            console.log(data);
            this.setState({ purchaseId: "", purchaseModalState: 'error' });
            return;
        }

        const id = this.state.prizes.findIndex((pr: any) => pr.id === prize.id);
        prize.quantity = prize.quantity - 1;

        const purchases = this.state.purchasedItems;
        purchases[prize.id] = data.id;
        //console.log(purchases);
        this.setState({
            purchaseId: data.id,
            purchaseModalState: 'loaded',
            purchasedItems: purchases,
            prizes: [
                ...this.state.prizes.slice(0, id),
                prize,
                ...this.state.prizes.slice(id + 1)
            ]
        });
    }
    /*
     * QR modal
     */
    openPurchasedQRModal = (prize: Prize) => {
        this.setState({
            purchaseModalOpen: true,
            purchaseModalState: 'loaded',
            purchaseId: this.state.purchasedItems[prize.id],
            prizeModalData: prize
        });
    }
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
        this.setState({ prizeModalData: prize, prizeModalOpen: true });
    }

    closePrizeModal = () => {
        this.setState({
            prizeModalOpen: false,
        });
    }

    public render() {
        let contents = this.state.loading
            ? <Loader active>Loading</Loader>
            : this.renderPrizeList(this.state.prizes);

        return (
            <div>
                <Container fluid>
                    <div style={{ marginLeft: 'auto', marginRight: 'auto', maxWidth: 225, paddingTop: 20 }}>
                        <Header as='h1' textAlign='center'>
                            <Icon name='gift' circular />
                            <Header.Content>
                                Prizes
                            </Header.Content>
                        </Header>
                    </div>
                </Container>
                <Divider />
                <Container fluid textAlign='center'>
                    <p>Visit a stand to collect purchased items</p>
                    <div style={{ height: 10 }} />
                </Container>
                <Container fluid>
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
                </Container>

            </div>
        );
    }
    private renderPrizeList(prizes: Prize[]) {
        return <div>
            <div className='row'>
                {prizes.map((prize, index) =>
                    <div className='col-xs-6 col-sm-4 col-md-3 col-lg-2 col-centered' key={index} style={{ paddingBottom: 20 }}>
                        <PrizeCard
                            prize={prize}
                            onBuy={this.openPrizeModal}
                            onOpenPurchaseQR={this.openPurchasedQRModal}
                            balance={200}
                            purchased={prize.id in this.state.purchasedItems}
                        />
                    </div>
                )}

            </div>
        </div>

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