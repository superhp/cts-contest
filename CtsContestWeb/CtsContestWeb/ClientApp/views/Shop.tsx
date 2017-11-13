import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Grid, Container, Header, Icon, Loader, Divider } from 'semantic-ui-react';
//https://react.semantic-ui.com/usage stylesheet missing
import { PrizeDescription } from '../components/PrizeDescription';
import { PrizeModal } from '../components/PrizeModal';
import { PurchaseModal } from '../components/PurchaseModal';
import { ShopItemCard } from '../components/ShopItemCard';
import 'isomorphic-fetch';

export class Shop extends React.Component<any, any> {
    _mounted: boolean;
    constructor() {
        super();
        this.state = {
            prizes: [], loading: true,
            prizeModalOpen: false,

            purchaseModalState: 'closed',
            purchaseModalOpen: false,
            purchase: {},
            purchasedItems: [],

            prizeModalData: {
                id: 0,
                name: "",
                price: 0,
                quantity: 0,
                picture: ""
            },

            prizeDescription: {},
            prizeDescriptionOpen: false
        };

    }

    componentDidMount() {
        fetch('api/Prize')
            .then(response => response.json() as Promise<Prize[]>)
            .then(data => {
                this.setState({ prizes: data, loading: false });
            });

        fetch('api/User/purchases', {
            credentials: 'include'
        })
            .then(response => response.json() as Promise<any>)
            .then(data => {
                if (this._mounted)
                    this.setState({ purchasedItems: data });
            });
        this._mounted = true;
    }

    componentWillUnmount() {
        this._mounted = false;
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
            }),
            credentials: 'include'
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

        const newPurchase = {
            prizeId: prize.id,
            price: prize.price,
            isGivenAway: false,
            purchaseId: data.id,
            userEmail: this.props.userInfo.email
        }
        purchases.push(newPurchase);

        this.props.onDecrementBalance(prize.price);

        this.setState({
            purchase: newPurchase,
            purchaseModalState: 'loaded',
            purchasedItems: purchases,
            prizes: [
                ...this.state.prizes.slice(0, id),
                prize,
                ...this.state.prizes.slice(id + 1)
            ]
        });
    }

    isPurchased(id: number) {
        const index = this.state.purchasedItems.findIndex((element: any) => {
            return element.prizeId === id;
        });
        if (index === -1) return false;
        return true;
    }
    findPrizePurchase(id: number) {
        const purchase = this.state.purchasedItems.find((element: any) => {
            return element.prizeId === id;
        });
        if (purchase !== undefined)
            return purchase;
        return {}
    }
    /*
     * QR modal
     */
    openPurchasedQRModal = (prize: Prize) => {
        this.setState({
            purchaseModalOpen: true,
            purchaseModalState: 'loaded',
            purchase: this.state.purchasedItems
                .find((element: any) =>
                    element.prizeId === prize.id),
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
    /*
     * Description modal
    */

    openDescriptionModal = (prize: any) => {
        this.setState({ prizeDescriptionOpen: true, prizeDescription: prize });
    }

    closeDescriptionModal = () => {
        this.setState({prizeDescriptionOpen: false});
    }
    public render() {
        let contents = this.state.loading
            ? <Loader active inline='centered'>Loading</Loader>
            : this.renderPrizeList(this.state.prizes.filter((prize:any) => prize.category === 'Prize for points'));

        return (
            <div className='cg-shop-page'>
                <div className='cg-page-header'>
                    <Container fluid>
                        <Header as='h1' textAlign='center' inverted>
                            <Icon name='tags' />
                            <Header.Content>
                                Shopping booth
                            </Header.Content>
                        </Header>
                    </Container>
                </div>
                <Container>
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
                        purchase={this.state.purchase} />
                    <PrizeDescription 
                        open={this.state.prizeDescriptionOpen} 
                        onClose={this.closeDescriptionModal}
                        prize={this.state.prizeDescription} />
                </Container>
                
            </div>
        );
    }
    private renderPrizeList(prizes: Prize[]) {
        return <div>
            <div className='cg-row last-not-grow'>
                {prizes.map((prize, index) =>
                    <div className='cg-col' key={index} style={{ paddingBottom: 20 }}>  
                        <ShopItemCard
                            prize={prize}
                            onBuy={this.openPrizeModal}
                            onOpenPurchaseQR={this.openPurchasedQRModal}
                            onOpenDescription={this.openDescriptionModal}
                            balance={this.props.userInfo.todaysBalance}
                            purchased={this.isPurchased(prize.id)}
                            userLogedIn={this.props.userInfo.isLoggedIn}
                            purchase={this.findPrizePurchase(prize.id)}
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
    category: string;
}

interface Purchase {
    id: string;
}