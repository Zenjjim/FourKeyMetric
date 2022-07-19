import { ChangeFailureRate } from "components/cfr/changeFailureRate";
import { ChangeFailureRateScore } from "components/cfr/changeFailureRateScore";
import { DeploymentFrequency } from "components/df/deploymentFrequency";
import { DeploymentFrequencyScore } from "components/df/deploymentFrequencyScore";
import { LeadTimeChange } from "components/ltc/leadTimeChange";
import { LeadTimeChangeScore } from "components/ltc/leadTimeChangeScore";
import { TimeToRestoreService } from "components/ttrs/timeToRestoreService";
import { TimeToRestoreServiceScore } from "components/ttrs/timeToRestoreServiceScore";
import { GetServerSideProps, InferGetServerSidePropsType } from "next";
import safeJsonStringify from "safe-json-stringify";
import styles from "styles/Home.module.css";

export const getServerSideProps: GetServerSideProps = async (context) => {
  if (!context.query?.months) {
    return {
      redirect: { permanent: false, destination: "/?months=3" },
      props: {},
    };
  }
  const backend = "http://localhost:5118";
  const months = context.query.months;
  const df = await fetch(
    `${backend}/DeploymentFrequency?intervalMonths=${months}`
  )
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    });
  const ltc = await fetch(`${backend}/LeadTimeChange?intervalMonths=${months}`)
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    });
  const cfr = await fetch(
    `${backend}/ChangeFailureRate?intervalMonths=${months}`
  )
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    });
  const ttrs = await fetch(
    `${backend}/RestoreServiceTime?intervalMonths=${months}`
  )
    .then((response) => response.json())
    .then((json) => JSON.parse(safeJsonStringify(json)))
    .catch(function (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    });
  return {
    props: { df, ltc, cfr, ttrs, months }, // will be passed to the page component as props
  };
};

const Home = ({
  df,
  ltc,
  cfr,
  ttrs,
  months,
}: InferGetServerSidePropsType<typeof getServerSideProps>) => {
  return (
    <div className={styles.container}>
      <TimeToRestoreServiceScore data={ttrs} months={months} />
      <ChangeFailureRateScore data={cfr} months={months} />
      <LeadTimeChangeScore data={ltc} months={months} />
      <DeploymentFrequencyScore data={df} months={months} />
      <TimeToRestoreService data={ttrs} months={months} />
      <ChangeFailureRate data={cfr} months={months} />
      <LeadTimeChange data={ltc} months={months} />
      <DeploymentFrequency data={df} months={months} />
    </div>
  );
};

export default Home;
