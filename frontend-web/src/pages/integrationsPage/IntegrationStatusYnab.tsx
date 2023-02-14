import React from "react";
import {useMatches} from "react-router-dom";

import Loader from "../../components/Loader";
import {useYnabConnectService} from "../../data/useYnabConnectService";
import {useYnabDisconnectService} from "../../data/useYnabDisconnectService";
import {useYnabStatusService} from "../../data/useYnabStatusService";
import withComponentErrorBoundary from "../../hoc/withComponentErrorBoundary";
import {paths} from "../../paths";
import redirectTo from "../../util/redirect";
import ConnectButton from "./ConnectButton";
import DisconnectButton from "./DisconnectButton";
import IntegrationReturnYnab, {getYnabReturnUrl} from "./IntegrationReturnYnab";
import logo from "./integrationStatusYnab/logo.svg"
import StatusConnected from "./StatusConnected";
import StatusContainer from "./StatusContainer";
import StatusDisconnected from "./StatusDisconnected";

const StatusWidget = withComponentErrorBoundary(() => {
    const returnUri = getYnabReturnUrl();
    const {
        data: statusData,
        isLoading: statusIsLoading
    } = useYnabStatusService();
    const {
        isLoading: disconnectIsLoading,
        refetch: disconnectRefetch
    } = useYnabDisconnectService();
    const {
        isLoading: connectIsLoading,
        isFetched: connectIsFetched,
        refetch: connectRefetch,
        data: connectData,
    } = useYnabConnectService(returnUri);
    const routeMatches = useMatches();
    const lastRouteMatch = routeMatches[routeMatches.length - 1];
    
    if (statusIsLoading) {
        return <Loader />;
    }
    
    if (!connectIsLoading && connectIsFetched && connectData?.data) {
        redirectTo(connectData.data);
    }
    
    if (lastRouteMatch.pathname === paths.integrations.return.ynab) {
        return (
            <StatusDisconnected>
                <IntegrationReturnYnab />
            </StatusDisconnected>
        );
    }
    
    if (statusData?.data?.connected) {
        return (
            <StatusConnected>
                <DisconnectButton loading={disconnectIsLoading} onClick={() => disconnectRefetch()} />
            </StatusConnected>
        );
    }

    return (
        <StatusDisconnected>
            <ConnectButton loading={connectIsLoading} onClick={() => connectRefetch()} />
        </StatusDisconnected>
    );
});

const IntegrationStatusYnab = () => {
    return (
        <StatusContainer icon={logo} name="ynab">
            <StatusWidget/>
        </StatusContainer>
    );
}

export default IntegrationStatusYnab;