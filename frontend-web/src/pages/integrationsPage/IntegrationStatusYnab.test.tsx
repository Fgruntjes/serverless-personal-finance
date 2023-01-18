import {jest} from "@jest/globals";
import {render, screen} from "@testing-library/react";

import {useYnabConnectService} from "../../data/useYnabConnectService";
import {useYnabDisconnectService} from "../../data/useYnabDisconnectService";
import {useYnabStatusService} from "../../data/useYnabStatusService";
import IntegrationStatusYnab from "./IntegrationStatusYnab";

jest.mock("../../generated/App.Function.Integration.Ynab");
jest.mock("../../data/useYnabStatusService");
jest.mock("../../data/useYnabConnectService");
jest.mock("../../data/useYnabDisconnectService");

describe(IntegrationStatusYnab.name, () => {
    const mockedUseStatusService = useYnabStatusService as jest.Mock<any>;
    const mockedUseDisconnectService = useYnabDisconnectService as jest.Mock<any>;
    const mockedUseConnectService = useYnabConnectService as jest.Mock<any>;
    
    test("Render loading status", async () => {
        mockedUseStatusService.mockReturnValue({isLoading: true});

        render(<IntegrationStatusYnab />);
        
        expect(screen.getByText("label.loading")).toBeInTheDocument();
    });
});