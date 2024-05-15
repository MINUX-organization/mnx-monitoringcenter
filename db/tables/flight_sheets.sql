-- Table: public.flight_sheet

-- DROP TABLE IF EXISTS public.flight_sheet;

CREATE TABLE IF NOT EXISTS public.flight_sheet
(
    id uuid NOT NULL,
    name text COLLATE pg_catalog."default" NOT NULL,
    miner_name text COLLATE pg_catalog."default" NOT NULL,
    cryptocurrency text COLLATE pg_catalog."default" NOT NULL,
    wallet_address text COLLATE pg_catalog."default" NOT NULL,
    pool_id uuid NOT NULL,
    user_id bigint NOT NULL,
    CONSTRAINT pk_flight_sheet PRIMARY KEY (id),
    CONSTRAINT fk_flight_sheet_miners_miner_name FOREIGN KEY (miner_name)
        REFERENCES public.miners (name) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    CONSTRAINT fk_flight_sheet_pools_pool_id FOREIGN KEY (pool_id)
        REFERENCES public.pools (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.flight_sheet
    OWNER to postgres;
-- Index: ix_flight_sheet_miner_name

-- DROP INDEX IF EXISTS public.ix_flight_sheet_miner_name;

CREATE INDEX IF NOT EXISTS ix_flight_sheet_miner_name
    ON public.flight_sheet USING btree
    (miner_name COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: ix_flight_sheet_pool_id

-- DROP INDEX IF EXISTS public.ix_flight_sheet_pool_id;

CREATE INDEX IF NOT EXISTS ix_flight_sheet_pool_id
    ON public.flight_sheet USING btree
    (pool_id ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: ix_flight_sheet_user_id

-- DROP INDEX IF EXISTS public.ix_flight_sheet_user_id;

CREATE INDEX IF NOT EXISTS ix_flight_sheet_user_id
    ON public.flight_sheet USING btree
    (user_id ASC NULLS LAST)
    TABLESPACE pg_default;