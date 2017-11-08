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
            <Modal.Header className='cg-modal-header'>
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
            <div className='cg-modal-actions'>
                <button
                    className='cg-card-button red'
                    onClick={this.close}
                >No</button>
                <button
                    className='cg-card-button secondary'
                    onClick={this.buy}
                >Yes</button>
            </div>
        )
    }
    public render() {
        return (
            <Modal open={this.props.open}
                size='tiny'
                closeOnEscape={true}
                closeOnRootNodeClick={true}
                style={{ backgroundColor: 'transparent', marginTop: -100 }}
                onClose={this.close}
                closeIcon
                ref={(content: any) => this.modal = content}
            >
                {this.renderHeader()}
                {this.renderContent()}
                {this.renderActions()}
            </Modal>
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