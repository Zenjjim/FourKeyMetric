import { GetServerSideProps, InferGetServerSidePropsType } from "next";
import safeJsonStringify from "safe-json-stringify";

import { ChangeFailureRate } from "components/changeFailureRate";
import { DeploymentFrequency } from "components/deploymentFrequency";
import { LeadTimeChange } from "components/leadTimeChange";
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
  return {
    props: { df, ltc, cfr, months }, // will be passed to the page component as props
  };
};

const Home = ({
  df,
  ltc,
  cfr,
  months,
}: InferGetServerSidePropsType<typeof getServerSideProps>) => {
  return (
    <div className={styles.container}>
      <ChangeFailureRate data={cfr} months={months} />
      <LeadTimeChange data={ltc} months={months} />
      <DeploymentFrequency data={df} months={months} />
    </div>
  );
};

export default Home;
