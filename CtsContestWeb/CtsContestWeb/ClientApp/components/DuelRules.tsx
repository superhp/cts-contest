import * as React from 'react';
import {Container} from 'semantic-ui-react';

interface IDuelRulesProps {
    duelState: any
}

const DuelRules : React.SFC<IDuelRulesProps> = (props) => {
    return (
        <Container>
            {!props.duelState.isInDuel ?
                <div>
                    <div className='ui label'>
                        Players in duel
                        <span className='detail'>{props.duelState.activePlayers}</span>
                    </div>
                    <div>
                        <div className='cg-infographic-container'>
                            <div className='cg-infographic'>
                                <i className='clock outline icon cg-infographic-icon'></i>
                                <div className='cg-infographic-text-container'>
                                    <div className='cg-infographic-title'>Wait</div>
                                    <div className='cg-infographic-text'>to get matched</div>
                                </div>
                            </div>
                            <div className='cg-infographic'>
                                <i className='laptop icon cg-infographic-icon'></i>
                                <div className='cg-infographic-text-container'>
                                    <div className='cg-infographic-title'>Solve</div>
                                    <div className='cg-infographic-text'>faster than opponent</div>
                                </div>
                            </div>
                            <div className='cg-infographic'>
                                <i className='trophy icon cg-infographic-icon'></i>
                                <div className='cg-infographic-text-container'>
                                    <div className='cg-infographic-title'>Win</div>
                                    <div className='cg-infographic-text'>and earn coins</div>
                                </div>
                            </div>
                        </div>

                        <div className='cg-title'>
                            <h2>Rules</h2>
                        </div>
                        <div className='cg-about-p'>
                            <ol>
                                <li><b>Opponents. </b>You can fight one opponent at a time. Opponents are matched randomly.</li>
                                <li><b>Tasks. </b>You and your opponent get the same randomly selected task. Both of you will be solving it for the first time - once you have
                                seen a particular task, it will not show up in your other duels.</li>
                                <li><b>Coins. </b>Duel tasks, depending on their difficulty, are worth 15, 20 or 40 coins. The first to solve the task wins. If nobody solves it in time,
                                no one is awarded.</li>
                                <li><b>Connection. </b>You will be able to continue the duel even if you lose connection. Once reconnected, go to the Duel page and press resume.</li>
                            </ol>
                        </div>
                    </div>
                </div>
                :
                <div></div>
            }
        </Container>
    );
}

export default DuelRules;