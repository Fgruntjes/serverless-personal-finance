import {jest} from "@jest/globals";
import {
    act, render, screen, waitFor
} from "@testing-library/react";
import {Simulate} from "react-dom/test-utils";

import {useYnabConnectService} from "../../data/useYnabConnectService";
import {useYnabDisconnectService} from "../../data/useYnabDisconnectService";
import {useYnabStatusService} from "../../data/useYnabStatusService";
import IntegrationStatusYnab from "./IntegrationStatusYnab";

import click = Simulate.click;

jest.mock("../../generated/App.Function.Integration.Ynab");
jest.mock("../../data/useYnabStatusService");
jest.mock("../../data/useYnabConnectService");
jest.mock("../../data/useYnabDisconnectService");

describe(IntegrationStatusYnab.name, () => {
    const mockedUseStatusService = useYnabStatusService as jest.Mock<any>;
    const mockedUseDisconnectService = useYnabDisconnectService as jest.Mock<any>;
    const mockedUseConnectService = useYnabConnectService as jest.Mock<any>;
    
    function mockStatusDisconnected() {
        mockedUseStatusService.mockReturnValue({
            isLoading: false,
            data: {data: {connected: false}}
        });
    }

    function mockStatusConnected() {
        mockedUseStatusService.mockReturnValue({
            isLoading: false,
            data: {data: {connected: true, name: "somename"}}
        });
    }
    
    test("Render loading status", async () => {
        mockedUseStatusService.mockReturnValue({isLoading: true});

        render(<IntegrationStatusYnab />);
        
        expect(screen.getByText("label.loading")).toBeInTheDocument();
    });

    test("Render connected", async () => {
        mockStatusConnected();
        mockedUseDisconnectService.mockReturnValue({isLoading: false})

        render(<IntegrationStatusYnab />);

        expect(screen.queryByText("label.loading")).not.toBeInTheDocument();
        expect(screen.getByText("label.connected")).toBeInTheDocument();
    });

    test("Render connected, click disconnect", async () => {
        const mockedStatusRefetch = jest.fn();
        mockedUseStatusService.mockReturnValue({
            isLoading: false,
            refetch: mockedStatusRefetch,
            data: {data: {connected: true, name: "somename"}}
        });
        mockedUseDisconnectService.mockReturnValue({
            isLoading: false,
            refetch: () => Promise.resolve(),
        })

        render(<IntegrationStatusYnab />);

        const button = screen.getByText("button.disconnect");
        expect(button).toBeInTheDocument();
        act(() => {
            click(button);
        });
        await waitFor(() => expect(mockedStatusRefetch).toHaveBeenCalledTimes(1));
    });

    test("Render connected, loading disconnect button", async () => {
        mockStatusConnected();
        mockedUseDisconnectService.mockReturnValue({isLoading: true})

        render(<IntegrationStatusYnab />);

        expect(screen.getByText("button.disconnect")).toHaveAttribute("disabled");
        expect(screen.getByText("label.connected")).toBeInTheDocument();
    });

    test("Render disconnected", async () => {
        mockStatusDisconnected();
        mockedUseConnectService.mockReturnValue({isLoading: false})
        
        render(<IntegrationStatusYnab />);

        expect(screen.queryByText("label.loading")).not.toBeInTheDocument();
        expect(screen.getByText("label.disconnected")).toBeInTheDocument();
    });

    test("Render disconnected, loading connect button", async () => {
        mockStatusDisconnected();
        mockedUseConnectService.mockReturnValue({isLoading: true})

        render(<IntegrationStatusYnab />);

        expect(screen.getByText("button.connect")).toHaveAttribute("disabled");
        expect(screen.getByText("label.disconnected")).toBeInTheDocument();
    });

    test("Render disconnected, click connect", async () => {
        mockStatusDisconnected();
        const mockedConnectRefetch = jest.fn();
        mockedUseConnectService.mockReturnValue({
            isLoading: false,
            refetch: mockedConnectRefetch,
        })

        render(<IntegrationStatusYnab />);

        const button = screen.getByText("button.connect");
        expect(button).toBeInTheDocument();
        act(() => {
            click(button);
        });

        expect(mockedConnectRefetch).toHaveBeenCalledTimes(1);
    });
});