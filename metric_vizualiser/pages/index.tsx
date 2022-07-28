import { ChangeFailureRate } from "components/cfr/changeFailureRate";
import { ChangeFailureRateScore } from "components/cfr/changeFailureRateScore";
import { DeploymentFrequency } from "components/df/deploymentFrequency";
import { DeploymentFrequencyScore } from "components/df/deploymentFrequencyScore";
import { Filter } from "components/Filter";
import { LeadTimeChange } from "components/ltc/leadTimeChange";
import { LeadTimeChangeScore } from "components/ltc/leadTimeChangeScore";
import { TimeToRestoreService } from "components/ttrs/timeToRestoreService";
import { TimeToRestoreServiceScore } from "components/ttrs/timeToRestoreServiceScore";
import { COLORS } from "const";
import { GetServerSideProps, InferGetServerSidePropsType } from "next";
import safeJsonStringify from "safe-json-stringify";

export const getServerSideProps: GetServerSideProps = async (context) => {
  if (!context.query?.months) {
    return {
      redirect: {
        permanent: false,
        destination: "/?months=3&organization=&project=&repository=",
      },
      props: {},
    };
  }
  const backend = "https://weu-dev-metric-app.azurewebsites.net/";
  // const backend = "http://localhost:5118";
  const months = context.query.months;
  const organization = context.query.organization;
  const project = context.query.project;
  const repository = context.query.repository;
  const filter = `organization=${organization}&project=${project}&repository=${repository}&intervalMonths=${months}`;
  const df = await fetch(`${backend}/DeploymentFrequency?${filter}`)
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    });
  const ltc = await fetch(`${backend}/LeadTimeChange?${filter}`)
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    });
  const cfr = await fetch(`${backend}/ChangeFailureRate?${filter}`)
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    });
  const ttrs = await fetch(`${backend}/RestoreServiceTime?${filter}`)
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    });
  const info = await fetch(`${backend}/info`)
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    });
  return {
    props: { df, ltc, cfr, ttrs, info, months }, // will be passed to the page component as props
  };
};

const plotStyle = {
  alignItems: "start",
  justifyContent: "center",
  display: "flex",
  background: COLORS.PAPER,
  borderRadius: "10px",
  overflow: "hidden",
};
const Home = ({
  df,
  ltc,
  cfr,
  ttrs,
  info,
  months,
}: InferGetServerSidePropsType<typeof getServerSideProps>) => {
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

export default Home;
