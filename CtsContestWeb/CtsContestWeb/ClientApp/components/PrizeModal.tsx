// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

import * as React from 'react';
import { Responsive, Button, Header, Icon, Modal } from 'semantic-ui-react';

interface PrizeModalProps {
    open: boolean;
    onClose: any;
    onBuy: any;
    prize: Prize;
}

interface PrizeModalState {
}

export class PrizeModal extends React.Component<PrizeModalProps, PrizeModalState> {
    modal: any;
    modalHeight: number;
    modalHeightSet = false;
    constructor() {
        super();

    }
    componentDidUpdate() {
        this.modalFix();
    }
    modalFix() {
        if (this.modal !== null && this.modal.ref !== undefined && this.modal.ref !== null) {
            this.modalHeight = 0;
            for (let i = 1; i < this.modal.ref.children.length; i++) {
                this.modalHeight += this.modal.ref.children[i].clientHeight;
            }
            if (!this.modalHeightSet)
                this.handleResize();
            this.modalHeightSet = true;
        }
    }
    handleResize = () => {
        this.setState({});
    }
    buy = () => {
        this.props.onBuy(this.props.prize);
        this.close();
    }
    close = () => {
        this.modalHeightSet = false;
        this.props.onClose();
    }
    public renderHeader() {
        return (
            <Modal.Header>
                {this.props.prize.name}
            </Modal.Header>
        )
    }
    public renderContent() {
        return (
            <Modal.Content>
                <Header>Are you sure you want to buy this item?</Header>
            </Modal.Content>
        )
    }
    public renderActions() {
        return (
            <Modal.Actions>
                <Button className='cg-bg-danger' inverted onClick={this.close}>
                    <Icon name='remove' /> No
                </Button>
                <Button className='cg-bg-success' inverted onClick={this.buy}>
                    <Icon name='checkmark' /> Yes
                </Button>

            </Modal.Actions>
        )
    }
    public render() {
        return (
            <Responsive onUpdate={this.handleResize}>
                <Modal open={this.props.open}
                    size='tiny'
                    closeOnEscape={true}
                    closeOnRootNodeClick={true}
                    style={{ height: window.innerHeight >= this.modalHeight ? this.modalHeight : 'auto' }}
                    onClose={this.close}
                    closeIcon
                    ref={(content: any) => this.modal = content}
                >
                    {this.renderHeader()}
                    {this.renderContent()}
                    {this.renderActions()}
                </Modal>
            </Responsive>
        )
    }
}

export interface Prize {
    id: number;
    name: string;
    price: number;
    quantity: number;
    picture: string;
}