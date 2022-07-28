import { ChangeFailureRate } from "components/cfr/changeFailureRate";
import { ChangeFailureRateScore } from "components/cfr/changeFailureRateScore";
import { DeploymentFrequency } from "components/df/deploymentFrequency";
import { DeploymentFrequencyScore } from "components/df/deploymentFrequencyScore";
import { Filter } from "components/filter";
import { LeadTimeChange } from "components/ltc/leadTimeChange";
import { LeadTimeChangeScore } from "components/ltc/leadTimeChangeScore";
import { TimeToRestoreService } from "components/ttrs/timeToRestoreService";
import { TimeToRestoreServiceScore } from "components/ttrs/timeToRestoreServiceScore";
import { COLORS } from "const";
import {
  IChangeFailureRate,
  IDeploymentFrequency,
  IInfo,
  ILeadTimeChange,
  ITimeToRestoreService,
} from "types";

const plotStyle = {
  alignItems: "start",
  justifyContent: "center",
  display: "flex",
  background: COLORS.PAPER,
  borderRadius: "10px",
  overflow: "hidden",
};
type DashboardProps = {
  df: IDeploymentFrequency;
  ltc: ILeadTimeChange;
  cfr: IChangeFailureRate;
  ttrs: ITimeToRestoreService;
  info: IInfo;
  months: number;
};
export const Dashboard = ({
  df,
  ltc,
  cfr,
  ttrs,
  info,
  months,
}: DashboardProps) => {
  return (
    <>
      <Filter info={info} />
      <div className="container">
        <div
          className="LeadTimeChangeScore"
          style={{
            justifyContent: "center",
            textAlign: "center",
            lineHeight: "normal",
            display: "flex",
            background: COLORS.PAPER,
            borderRadius: "10px",
          }}
        >
          <LeadTimeChangeScore data={ltc} />
        </div>
        <div
          className="DeploymentFrequencyScore"
          style={{
            justifyContent: "center",
            textAlign: "center",
            lineHeight: "normal",
            display: "flex",
            background: COLORS.PAPER,
            borderRadius: "10px",
          }}
        >
          <DeploymentFrequencyScore data={df} />
        </div>
        <div
          className="TimeToRestoreServiceScore"
          style={{
            justifyContent: "center",
            textAlign: "center",
            lineHeight: "normal",
            display: "flex",
            background: COLORS.PAPER,
            borderRadius: "10px",
          }}
        >
          <TimeToRestoreServiceScore data={ttrs} />
        </div>
        <div
          className="ChangeFailureRateScore"
          style={{
            justifyContent: "center",
            textAlign: "center",
            lineHeight: "normal",
            display: "flex",
            background: COLORS.PAPER,
            borderRadius: "10px",
          }}
        >
          <ChangeFailureRateScore data={cfr} />
        </div>
        <div className="LeadTimeChange" style={plotStyle}>
          <LeadTimeChange data={ltc} months={months} />
        </div>
        <div className="DeploymentFrequency" style={plotStyle}>
          <DeploymentFrequency data={df} months={months} />
        </div>
        <div className="TimeToRestoreService" style={plotStyle}>
          <TimeToRestoreService data={ttrs} months={months} />
        </div>
        <div className="ChangeFailureRate" style={plotStyle}>
          <ChangeFailureRate data={cfr} months={months} />
        </div>
      </div>
    </>
  );
};
