-- Postgres init example
drop view if exists public.word_vw;
drop materialized view if exists public.top10_hash;

drop table if exists public.cashe_state;
drop table if exists public."event";
drop table if exists public."token";
drop table if exists public.word;

CREATE TABLE public.word (
	hash varchar NOT NULL,
	source_lang varchar NOT NULL,
	target_lang varchar NOT NULL,
	source_text varchar NOT NULL,
	target_text varchar NOT NULL,
	CONSTRAINT word_pk PRIMARY KEY (hash)
);
CREATE UNIQUE INDEX word_hash_idx ON public.word USING btree (hash);
CREATE INDEX word_source_idx ON public.word USING btree (source_lang);
CREATE INDEX word_target_idx ON public.word USING btree (target_lang);

CREATE TABLE public.cashe_state (
	x_memcache_rem int4 DEFAULT 0 NOT NULL,
	x_dbcache_rem int4 DEFAULT 0 NOT NULL,
	x_db_bytes int8 DEFAULT 0 NOT NULL,
	x_mem_bytes int8 DEFAULT 0 NOT NULL,
	x_req_rem int4 DEFAULT 0 NOT NULL
);

CREATE TABLE public."token" (
	token_value varchar NULL
);

CREATE TABLE public."event" (
	word_hash varchar NOT NULL,
	event_time timestamp NOT NULL,
	event_id bigserial NOT NULL,
	CONSTRAINT events_pk PRIMARY KEY (event_id),
	CONSTRAINT events_word_fk FOREIGN KEY (word_hash) REFERENCES public.word(hash)
);

CREATE MATERIALIZED VIEW public.top10_hash
TABLESPACE pg_default
AS SELECT word_hash AS hash,
    count(1) AS cnt
   FROM event e
  GROUP BY word_hash
  ORDER BY (count(1)) DESC
 LIMIT 10
WITH DATA;

CREATE UNIQUE INDEX top10_hash_hash_idx ON public.top10_hash USING btree (hash);

CREATE OR REPLACE VIEW public.word_vw
AS SELECT w.hash,
    w.source_lang,
    w.target_lang,
    w.source_text,
    w.target_text
   FROM word w
     JOIN top10_hash th ON w.hash::text = th.hash::text;