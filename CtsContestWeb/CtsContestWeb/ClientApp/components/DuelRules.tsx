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
                    <div className='cg-about-p'>
                        <div>Players in duel: {props.duelState.activePlayers}.</div>
                    </div>
                    <div className='cg-about-p'>
                        <div>Your statistics: {props.duelState.totalWins} wins, {props.duelState.totalLooses} loses.</div>
                    </div>
                    <div>
                        <div className='duel-how-to-container'>
                            <div className='duel-how-to'>
                                <i className='clock outline icon duel-how-to-icon'></i>
                                <div className='duel-how-to-text-container'>
                                    <div className='duel-how-to-title'>Wait</div>
                                    <div className='duel-how-to-text'>to get matched</div>
                                </div>
                            </div>
                            <div className='duel-how-to'>
                                <i className='laptop icon duel-how-to-icon'></i>
                                <div className='duel-how-to-text-container'>
                                    <div className='duel-how-to-title'>Solve</div>
                                    <div className='duel-how-to-text'>faster than opponent</div>
                                </div>
                            </div>
                            <div className='duel-how-to'>
                                <i className='trophy icon duel-how-to-icon'></i>
                                <div className='duel-how-to-text-container'>
                                    <div className='duel-how-to-title'>Win</div>
                                    <div className='duel-how-to-text'>and earn coins</div>
                                </div>
                            </div>
                        </div>

                        <div className='cg-title'>
                            <h2>Rules</h2>
                        </div>
                        <div className='cg-about-p'>
                            <ol>
                                <li>You can fight only one opponent at a time.</li>
                                <li>You can solve a task only once. That means you can duel until complete all the duel
                                    tasks.
                                </li>
                                <li>Don't worry if you loose your connection. The progress won't be lost and you
                                    will be able to jump back into the duel.
                                </li>
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