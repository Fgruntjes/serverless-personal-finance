import React from "react";
import {useQuery} from "react-query";

import withComponentErrorBoundary from "../../components/withComponentErrorBoundary";
import {ConnectService, StatusService} from "../../generated/functionIntegrationYnab";
import ConnectButton from "./ConnectButton";
import DisconnectButton from "./DisconnectButton";
import logo from "./integrationStatusYnab/logo.svg"
import StatusConnected from "./StatusConnected";
import StatusContainer from "./StatusContainer";
import StatusDisconnected from "./StatusDisconnected";
import StatusLoader from "./StatusLoader";

const YnabConnectButton = withComponentErrorBoundary(() => {
    function onConnect() {
        ConnectService
            .connect()
            .then(response => {
                if (response.data) {
                    window.location.href = response.data;
                }
            });
    }

    return <ConnectButton onClick={() => onConnect()} />;
});

function YnabDisconnectButton() {
    function onDisconnect() {
        
    }

    return <DisconnectButton onClick={() => onDisconnect()} />;
}

const StatusWidget = withComponentErrorBoundary(() => {
    const {data: statusData, isLoading: statusIsLoading} = useQuery("IntegrationStatusYnab",() => StatusService.status());

    if (statusIsLoading) {
        return <StatusLoader />;
    }

    if (statusData?.connected) {
        return <StatusConnected><YnabDisconnectButton /></StatusConnected>;
    } else {
        return <StatusDisconnected><YnabConnectButton /></StatusDisconnected>;
    }
});

const IntegrationStatusYnab = () => (
    <StatusContainer icon={logo} name="ynab">
        <StatusWidget />
    </StatusContainer>
);

export default IntegrationStatusYnab;