import * as React from 'react';

import { Button, Header, Image, Modal } from 'semantic-ui-react'
export class PrizeDescription extends React.Component<any, any> {
    constructor(props: any) {
        super(props);

    }
    close = () =>{
        this.props.onClose();
    }
    render() {
        return (
            <Modal className='cg-modal'
                open={this.props.open}
                closeOnEscape={true}
                closeOnRootNodeClick={true}
                onClose={this.close}
                closeIcon>
                <Modal.Header>{this.props.prize.name}</Modal.Header>
                <Modal.Content>
                    <Image className='cg-modal-image' wrapped size='medium' src={this.props.prize.picture} />
                    <Modal.Description>
                        <div className='cg-modal-description' dangerouslySetInnerHTML={{ __html: this.props.prize.description }}>

                        </div>
                    </Modal.Description>
                </Modal.Content>
            </Modal>
        )
    }
}