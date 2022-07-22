import { ChangeFailureRate } from "components/cfr/changeFailureRate";
import { ChangeFailureRateScore } from "components/cfr/changeFailureRateScore";
import { DeploymentFrequency } from "components/df/deploymentFrequency";
import { DeploymentFrequencyScore } from "components/df/deploymentFrequencyScore";
import { LeadTimeChange } from "components/ltc/leadTimeChange";
import { LeadTimeChangeScore } from "components/ltc/leadTimeChangeScore";
import { TimeToRestoreService } from "components/ttrs/timeToRestoreService";
import { TimeToRestoreServiceScore } from "components/ttrs/timeToRestoreServiceScore";
import { GetServerSideProps, InferGetServerSidePropsType } from "next";
import { useLayoutEffect, useRef, useState } from "react";
import safeJsonStringify from "safe-json-stringify";

import {
  Drawer,
  DrawerBody,
  DrawerFooter,
  DrawerHeader,
  DrawerOverlay,
  DrawerContent,
  DrawerCloseButton,
} from "@chakra-ui/react";
import { COLORS } from "const";

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

const scoreStyle = {
  justifyContent: "center",
  textAlign: "center",
  lineHeight: "normal",
  display: "flex",
  background: COLORS.PAPER,
  borderRadius: "10px",
};
const plotStyle = {
  alignItems: "start",
  justifyContent: "center",
  display: "flex",
  background: COLORS.PAPER,
  borderRadius: "10px",
};
const Home = ({
  df,
  ltc,
  cfr,
  ttrs,
  months,
}: InferGetServerSidePropsType<typeof getServerSideProps>) => {
  const refContainer = useRef();
  const refScoreContainer = useRef();
  const [size, setSize] = useState<{ width: number; height: number }>({
    width: 640,
    height: 400,
  });
  const [textSize, setTextSize] = useState<number>(25);
  useLayoutEffect(() => {
    if (refContainer.current) {
      setSize({
        width: refContainer.current.clientWidth,
        height: refContainer.current.clientWidth * (280 / 700),
      });
      setTextSize(refScoreContainer.current?.clientWidth / 6);
    }
  }, [refContainer]);
  return (
    <div className="container">
      <div
        ref={refScoreContainer}
        className="LeadTimeChangeScore"
        style={scoreStyle}
      >
        <LeadTimeChangeScore data={ltc} textSize={textSize} />
      </div>
      <div className="DeploymentFrequencyScore" style={scoreStyle}>
        <DeploymentFrequencyScore data={df} textSize={textSize} />
      </div>
      <div className="TimeToRestoreServiceScore" style={scoreStyle}>
        <TimeToRestoreServiceScore data={ttrs} textSize={textSize} />
      </div>
      <div className="ChangeFailureRateScore" style={scoreStyle}>
        <ChangeFailureRateScore data={cfr} textSize={textSize} />
      </div>
      <div ref={refContainer} className="LeadTimeChange" style={plotStyle}>
        <LeadTimeChange data={ltc} months={months} size={size} />
      </div>
      <div className="DeploymentFrequency" style={plotStyle}>
        <DeploymentFrequency data={df} months={months} size={size} />
      </div>
      <div className="TimeToRestoreService" style={plotStyle}>
        <TimeToRestoreService data={ttrs} months={months} size={size} />
      </div>
      <div className="ChangeFailureRate" style={plotStyle}>
        <ChangeFailureRate data={cfr} months={months} size={size} />
      </div>
      {/* <div
        style={{
          width: "60px",
          height: "60px",
          borderRadius: "50%",
          color: "black",
          background: "lightblue",
          position: "absolute",
          textAlign: "center",
          justifyContent: "center",
          alignItems: "center",
          display: "flex",
          fontSize: "3rem",
          bottom: 20,
          right: 20,
        }}
      >
        ⚙
      </div> */}
    </div>
  );
};

export default Home;
