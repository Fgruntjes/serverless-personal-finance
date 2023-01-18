import React from "react";

import Loader from "../../components/Loader";
import {useYnabConnectService} from "../../data/useYnabConnectService";
import {useYnabDisconnectService} from "../../data/useYnabDisconnectService";
import {useYnabStatusService} from "../../data/useYnabStatusService";
import withComponentErrorBoundary from "../../hoc/withComponentErrorBoundary";
import redirectTo from "../../util/redirect";
import ConnectButton from "./ConnectButton";
import DisconnectButton from "./DisconnectButton";
import {getYnabReturnUrl} from "./IntegrationReturnYnab";
import logo from "./integrationStatusYnab/logo.svg"
import StatusConnected from "./StatusConnected";
import StatusContainer from "./StatusContainer";
import StatusDisconnected from "./StatusDisconnected";

const YnabConnectButton = withComponentErrorBoundary(() => {
    const returnUri = getYnabReturnUrl();
    const {
        isLoading, refetch, data, isFetched, isError
    } = useYnabConnectService(returnUri);

    if (isFetched && !isError && data && data.data) {
        redirectTo(data.data);
        return <ConnectButton loading={true} />;
    }
    
    return <ConnectButton loading={isLoading} onClick={() => refetch()} />;
});

function YnabDisconnectButton(props: {onDisconnect: () => void}) {
    const {isLoading, refetch} = useYnabDisconnectService();

    return <DisconnectButton loading={isLoading} onClick={() => refetch().then(props.onDisconnect)} />;
}

const StatusWidget = withComponentErrorBoundary(() => {
    const {
        data, isLoading, refetch
    } = useYnabStatusService();
    
    if (isLoading) {
        return <Loader />;
    }

    if (data?.data?.connected) {
        return <StatusConnected><YnabDisconnectButton onDisconnect={refetch} /></StatusConnected>;
    } else {
        return <StatusDisconnected><YnabConnectButton /></StatusDisconnected>;
    }
});

const IntegrationStatusYnab = () => (
    <StatusContainer icon={logo} name="ynab">
        <StatusWidget/>
    </StatusContainer>
);

export default IntegrationStatusYnab;